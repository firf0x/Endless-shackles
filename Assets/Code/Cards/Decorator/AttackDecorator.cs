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
        private StrategyHandler<IAttackStrategy> strategyHandle;

        public AttackDecorator(int damageValue, IAttackStrategy strategy, IDeck<CardData> deck, ICard<CardTypeEnum> card) : base(card)
        {
            this.defaultDamageValue = damageValue;
            ChangeDamage(defaultDamageValue);

            defenceDeck = deck;
            strategyHandle = new StrategyHandler<IAttackStrategy>(strategy);
        }

        public override void Start()
        {
            base.Start();
            if (Parent.TryGetComponent<CardData>(out var data) && data.TryGetCardFeature<StepCombatDecorator>(out var decorator)) decorator.OnStepInteraction += OnStep;
        }

        // Этот метод нужон только для монстров
        private void OnStep()
        {
            GameObject target = null;

            // Ищем первую карту в defenceDeck, у которой есть HealthDecorator
            foreach (var card in defenceDeck.cardDatas)
            {
                if (card != null && card.TryGetCardFeature<HealthDecorator>(out _))
                {
                    target = card.gameObject;
                    break;
                }
            }

            if (defenceDeck.GetCardCount() == 0)
            {
                PlayerSystem.Instance.Kill();
                return;
            }

            Use(target);
        }

        public override void Use(GameObject target)
        {
            base.Use(target);
            strategyHandle.ExecuteStrategy(Parent, target, currentDamage.Value);
        }
        public void ChangeDamage(int value)
        {
            currentDamage.Value = value;
            currentDamage.Value = Mathf.Max(0, currentDamage.Value);
        }

        public void ChangeStrategy(StrategyAttackBase newStrategy) => strategyHandle.ChangeStrategy(newStrategy);

        public override string ToString()
        {
            string message = $"Attack: было нанесено {currentDamage.Value} урона.";
            Debug.Log(message);
            return message;
        }

        public override void Dispose()
        {
            if (Parent.TryGetComponent<CardData>(out var data) && data.TryGetCardFeature<StepCombatDecorator>(out var decorator)) decorator.OnStepInteraction -= OnStep;
            strategyHandle.Dispose();
            base.Dispose();
        }
    }
}