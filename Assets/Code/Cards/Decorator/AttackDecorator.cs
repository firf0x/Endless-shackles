using Game.Cards.Strategy;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class AttackDecorator : CardDecorator
    {
        public readonly int defaultDamageValue;
        public ReactiveProperty<int> currentDamage { get; private set; } = new();
        private IDeck<CardData> defenceDeck;
        private StrategyHandler<IAttackStrategy, CombatArgs> strategyHandle;

        private GameObject currentTarget = null;

        public AttackDecorator(int damageValue, IAttackStrategy strategy, IDeck<CardData> deck, ICard card) : base(card)
        {
            this.defaultDamageValue = damageValue;
            ChangeDamage(defaultDamageValue);

            defenceDeck = deck;
            strategyHandle = new StrategyHandler<IAttackStrategy, CombatArgs>(strategy);
        }

        public override void Start()
        {
            base.Start();
            if (!Type.HasFlag(CardTypeEnum.Attack) && CardData.TryGetCardFeature<StepCombatDecorator>(out var decorator))
            {
                decorator.OnStepStartUpdate += OnStep;
                decorator.OnStepEndUpdate += Attack;
            }
        }

        // Этот метод нужон только для монстров
        private void OnStep()
        {
            if (defenceDeck.GetCardCount() == 0)
            {
                PlayerSystem.Instance.Kill();
                return;
            }

            // Ищем первую карту в defenceDeck, у которой есть HealthDecorator
            foreach (var card in defenceDeck.cardDatas)
            {
                if (card != null && card.GetCardFeature<HealthDecorator>().isTarget)
                {
                    currentTarget = card.gameObject;
                    break;
                }
            }
        }

        // Нужен для карт атаки и только они могут вызвать его нормально
        public override void Use(GameObject target)
        {
            base.Use(target);

            if(target.GetComponent<CardData>().decorateCard.Type.HasFlag(CardTypeEnum.Defence)) return;

            if(Type.HasFlag(CardTypeEnum.Attack) && target.GetComponent<CardData>().GetCardFeature<HealthDecorator>().isTarget)
            {
                strategyHandle.ExecuteStrategy(new CombatArgs(CardData, target.GetComponent<CardData>(), currentDamage.Value));
            }
        }

        // Этот метод нужон только для монстров
        private void Attack()
        {
            if(currentTarget != null) strategyHandle.ExecuteStrategy(new CombatArgs(CardData, currentTarget.GetComponent<CardData>(), currentDamage.Value));
        }

        public void ChangeDamage(int value)
        {
            if(currentDamage.Value == value) return;
            currentDamage.Value = Mathf.Max(0, value);
        }

        public void ChangeTarget(GameObject target)
        {
            currentTarget = target;
        }

        public void ChangeStrategy(IAttackStrategy newStrategy) => strategyHandle.ChangeStrategy(newStrategy);

        public override void Dispose()
        {
            if (CardData != null && CardData.TryGetCardFeature<StepCombatDecorator>(out var decorator))
            {
                decorator.OnStepStartUpdate -= OnStep;
                decorator.OnStepEndUpdate -= Attack;
            }
            strategyHandle.Dispose();

            base.Dispose();
        }
    }
}