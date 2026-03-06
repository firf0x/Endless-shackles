using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    #region Standard Attack Strategies

    public class StandartAttackStategy : StrategyAttackBase
    {
        public override string Name => "Default Attack";
        public override ICard<CardTypeEnum> Card { get; protected set; }

        private GameObject target;
        private int currentDamageValue;
        private CardTypeEnum ignoreLayers;

        public StandartAttackStategy(GameObject target, GameObject parent, int damageValue, CardTypeEnum ignoreLayers)
        {
            this.target = target;
            this.currentDamageValue = damageValue;
            this.ignoreLayers = ignoreLayers;
        }

        public override void Execute()
        {
            CardData data = target.GetComponent<CardData>();

            if (data.TryGetCardFeature<HealthDecorator>(out var feature) && !data.decorateCard.IgnoreLayers.HasFlag(ignoreLayers))
            {
                // Удаление карты атаки при нанесении урона по карте монстра
                if (data.decorateCard.Type.HasFlag(CardTypeEnum.Monster))
                {
                    (Card as CardData)?.CardDestroy(false);
                }

                feature.TakeDamage(currentDamageValue);
                StepCombatSystem.Instance.StepUpdate();
            }
        }

        public override void UpdateTarget(GameObject newTarget) => target = newTarget;
        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    public class MonsterAttackStrategy : DefenceDeckAttackStrategy
    {
        public override string Name => "Default Attack";
        public override ICard<CardTypeEnum> Card { get; protected set; }

        public MonsterAttackStrategy(PlayerSystem player, int damageValue, IDeck<CardData> defenceDeck)
            : base(player, damageValue, defenceDeck)
        {
            Card = null; // в данной стратегии нет привязанной карты
        }

        // Используется полностью базовое поведение: атака первой попавшейся карты защиты
    }

    #endregion

    #region Modifiers Attack

    public class IgnoreDefenceTypeAttackStategy : DefenceDeckAttackStrategy
    {
        public override string Name => "Ignore defence type attack";
        public override ICard<CardTypeEnum> Card { get; protected set; }

        private readonly DefenceType type;
        private CardData lastDefenceCard; // последняя просмотренная карта (на случай, если все нужного типа)

        public IgnoreDefenceTypeAttackStategy(PlayerSystem player, DefenceType defenceType, int damageValue, IDeck<CardData> defendDeck)
            : base(player, damageValue, defendDeck)
        {
            type = defenceType;
            Card = null;
        }

        protected override bool IsTarget(CardData card)
        {
            // Запоминаем карту для возможного использования в HandleNoTarget
            lastDefenceCard = card;
            // Цель – карта, у которой тип защиты НЕ равен заданному
            return card.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType != type;
        }

        protected override void HandleNoTarget()
        {
            // Если все карты имеют игнорируемый тип защиты – убить игрока
            if (lastDefenceCard != null)
                Player.Kill();
        }
    }

    public class MultiplyDamageStrategy : DefenceDeckAttackStrategy
    {
        public override string Name => "Default Attack";
        public override ICard<CardTypeEnum> Card { get; protected set; }

        private readonly AttackDecorator currentCard;

        public MultiplyDamageStrategy(PlayerSystem player, int damageValue, IDeck<CardData> defenceDeck, AttackDecorator currentCard)
            : base(player, damageValue, defenceDeck)
        {
            this.currentCard = currentCard;
            Card = null; // при необходимости можно получить карту из currentCard, если она там есть
        }

        protected override void ApplyDamage(CardData target)
        {
            base.ApplyDamage(target);
            currentCard.ChangeDamage(DamageValue);
        }
    }

    public class MultiplyDamageByTypeStrategy : DefenceDeckAttackStrategy
    {
        public override string Name => "Default Attack";
        public override ICard<CardTypeEnum> Card { get; protected set; }

        private readonly DefenceType type;

        public MultiplyDamageByTypeStrategy(PlayerSystem player, int damageValue, IDeck<CardData> defenceDeck, DefenceType defenceType)
            : base(player, damageValue, defenceDeck)
        {
            type = defenceType;
            Card = null;
        }

        protected override bool IsTarget(CardData card)
        {
            // Если тип совпадает – урон удваивается (обрабатывается в ApplyDamage)
            return true; // всегда выбираем первую карту, но модифицируем урон в ApplyDamage
        }

        protected override void ApplyDamage(CardData target)
        {
            if (target.TryGetCardFeature<HealthDecorator>(out var health))
            {
                int finalDamage = type == target.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType
                    ? DamageValue * 2
                    : DamageValue;
                health.TakeDamage(finalDamage);
            }
        }
    }

    public class LoopAttackByStepStrategy : DefenceDeckAttackStrategy
    {
        public override string Name => "Default Attack";
        public override ICard<CardTypeEnum> Card { get; protected set; }

        public LoopAttackByStepStrategy(PlayerSystem player, int damageValue, IDeck<CardData> defenceDeck)
            : base(player, damageValue, defenceDeck)
        {
            Card = null;
        }

        // Полностью базовое поведение
    }

    public class DeckStrikeAttackRegenerateStrategy : StrategyAttackBase
    {
        public override string Name => "Deck strike";
        public override ICard<CardTypeEnum> Card { get; protected set; }

        private readonly PlayerSystem player;
        private int currentDamageValue;
        private readonly int currentHealValue;
        private readonly IDeck<CardData> defenceDeck;
        private readonly IDeck<CardData> monsterDeck;

        public DeckStrikeAttackRegenerateStrategy(PlayerSystem player, int damageValue, int regenerateHP,
            IDeck<CardData> defenceDeck, IDeck<CardData> monsterDeck, ICard<CardTypeEnum> card)
        {
            this.player = player;
            currentDamageValue = damageValue;
            currentHealValue = regenerateHP;
            this.defenceDeck = defenceDeck;
            this.monsterDeck = monsterDeck;
            Card = card;
        }

        public override void Execute()
        {
            if (defenceDeck.GetCardCount() == 0)
            {
                player.Kill();
                return;
            }

            int multiply = 0;
            foreach (var card in defenceDeck.cardDatas)
            {
                if (card?.decorateCard == null) continue;
                if (card.TryGetCardFeature<HealthDecorator>(out var decorator))
                {
                    multiply++;
                    decorator.TakeDamage(currentDamageValue);
                }
            }

            for (int i = 0; i < multiply; i++)
            {
                foreach (var card in monsterDeck.cardDatas)
                {
                    if (card?.gameObject == Card?.Parent) continue; // пропускаем самого себя
                    card?.GetCardFeature<HealthDecorator>()?.healthSystem.Heal(currentHealValue);
                }
            }
        }

        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    #endregion
}