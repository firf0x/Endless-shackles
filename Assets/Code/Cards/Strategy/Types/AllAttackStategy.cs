using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    #region Standard Attack Strategies

    public class StandartAttackStategy : StrategyAttackBase
    {
        public override string Name => "Default Attack";
        private GameObject target;
        private GameObject parent;
        private int currentDamageValue;
        private CardTypeEnum ignoreLayers;

        public StandartAttackStategy(GameObject target, GameObject parent, int damageValue, CardTypeEnum ignoreLayers)
        {
            this.target = target;
            this.parent = parent;
            currentDamageValue = damageValue;
            this.ignoreLayers = ignoreLayers;
        }

        public override bool IsValidTarget(GameObject target)
        {
            if (!base.IsValidTarget(target)) return false;
            var data = target.GetComponent<CardData>();
            // Цель не должна быть в игнорируемом слое
            return !data.decorateCard.IgnoreLayers.HasFlag(ignoreLayers);
        }

        public override void Execute()
        {
            if (target == null) return;

            var data = target.GetComponent<CardData>();
            if (data.TryGetCardFeature<HealthDecorator>(out var feature))
            {
                // Удаление карты атаки при нанесении урона монстру
                if (data.decorateCard.Type.HasFlag(CardTypeEnum.Monster))
                {
                    parent.GetComponent<CardData>().CardDestroy(false);
                }

                feature.TakeDamage(currentDamageValue);
                StepCombatSystem.Instance.StepUpdate();
            }
        }

        public override void UpdateTarget(GameObject newTarget) => target = newTarget;
        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    public class MonsterAttackStrategy : StrategyAttackBase
    {
        public override string Name => "Default Attack";
        private PlayerSystem player;
        private int currentDamageValue;
        private GameObject target;

        public MonsterAttackStrategy(PlayerSystem player, int damageValue)
        {
            this.player = player;
            currentDamageValue = damageValue;
        }

        public override void Execute()
        {
            if (target == null)
            {
                player.Kill();
                return;
            }

            if (target.TryGetComponent<CardData>(out var card) &&
                card.TryGetCardFeature<HealthDecorator>(out var health))
            {
                health.TakeDamage(currentDamageValue);
            }
        }

        public override void UpdateTarget(GameObject newTarget) => target = newTarget;
        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    #endregion

    #region Modifier Attack Strategies

    public class IgnoreDefenceTypeAttackStategy : StrategyAttackBase
    {
        public override string Name => "Ignore defence type attack";
        private int currentDamageValue;
        private PlayerSystem player;
        private DefenceType type;
        private GameObject target;

        public IgnoreDefenceTypeAttackStategy(PlayerSystem player, DefenceType defenceType, int damageValue)
        {
            this.player = player;
            currentDamageValue = damageValue;
            type = defenceType;
        }

        public override bool IsValidTarget(GameObject target)
        {
            if (!base.IsValidTarget(target)) return false;
            var defenceType = target.GetComponent<CardData>()?.GetCardFeature<CustomTypeDecorator<DefenceType>>();
            return defenceType != null && defenceType.CustomType != type;
        }

        public override void Execute()
        {
            if (target == null)
            {
                player.Kill();
                return;
            }

            if (target.TryGetComponent<CardData>(out var card) &&
                card.TryGetCardFeature<HealthDecorator>(out var health))
            {
                health.TakeDamage(currentDamageValue);
            }
        }

        public override void UpdateTarget(GameObject newTarget) => target = newTarget;
        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    public class DeckStrikeAttackRegenerateStrategy : StrategyAttackBase
    {
        public override string Name => "Deck strike";
        private PlayerSystem player;
        private int currentDamageValue;
        private int currentHealValue;
        private IDeck<CardData> monsterDeck;
        private ICard<CardTypeEnum> objectCard;
        private GameObject target;

        public DeckStrikeAttackRegenerateStrategy(PlayerSystem player, int damageValue, int regenerateHP,
                                                  IDeck<CardData> monsterDeck, ICard<CardTypeEnum> card)
        {
            this.player = player;
            currentDamageValue = damageValue;
            currentHealValue = regenerateHP;
            this.monsterDeck = monsterDeck;
            objectCard = card;
        }

        public override void Execute()
        {
            if (target == null)
            {
                player.Kill();
                return;
            }

            // Наносим урон одной цели
            if (target.TryGetComponent<CardData>(out var targetCard) &&
                targetCard.TryGetCardFeature<HealthDecorator>(out var health))
            {
                health.TakeDamage(currentDamageValue);
            }

            // Лечим всех монстров (кроме самого себя) один раз
            foreach (var card in monsterDeck.cardDatas)
            {
                if (card == null || card.gameObject == objectCard.Parent) continue;
                if (card.TryGetCardFeature<HealthDecorator>(out var monsterHealth))
                {
                    monsterHealth.healthSystem.Heal(currentHealValue);
                }
            }
        }

        public override void UpdateTarget(GameObject newTarget) => target = newTarget;
        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    public class MultiplyDamageStrategy : StrategyAttackBase
    {
        public override string Name => "Multiply damage Attack";
        private PlayerSystem player;
        private AttackDecorator currentCard;
        private GameObject target;

        public MultiplyDamageStrategy(PlayerSystem player, AttackDecorator currentCard)
        {
            this.player = player;
            this.currentCard = currentCard;
        }

        public override void Execute()
        {
            if (target == null)
            {
                player.Kill();
                return;
            }

            if (target.GetComponent<CardData>().TryGetCardFeature<HealthDecorator>(out var decorator)) decorator.TakeDamage(currentCard.currentDamage.Value * 2);
        }

        public override void UpdateTarget(GameObject newTarget) => target = newTarget;
    }

    public class MultiplyDamageByTypeStrategy : StrategyAttackBase
    {
        public override string Name => "Multiply by type Attack";
        private PlayerSystem player;
        private int currentDamageValue;
        private DefenceType type;
        private GameObject target;

        public MultiplyDamageByTypeStrategy(PlayerSystem player, int damageValue, DefenceType defenceType)
        {
            this.player = player;
            this.currentDamageValue = damageValue;
            this.type = defenceType;
        }

        public override void Execute()
        {
            if (target == null)
            {
                player.Kill();
                return;
            }

            if (!target.TryGetComponent<CardData>(out var card) ||
                !card.TryGetCardFeature<HealthDecorator>(out var health))
                return;

            int damage = currentDamageValue;
            var defenceType = card.GetCardFeature<CustomTypeDecorator<DefenceType>>();
            if (defenceType != null && defenceType.CustomType == type)
                damage *= 2;

            health.TakeDamage(damage);
        }

        public override void UpdateTarget(GameObject newTarget) => target = newTarget;
        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    public class LoopAttackByStepStrategy : StrategyAttackBase
    {
        public override string Name => "Default Attack";
        private int currentDamageValue;
        private GameObject target;

        public LoopAttackByStepStrategy(int damageValue)
        {
            currentDamageValue = damageValue;
        }

        public override void Execute()
        {
            if (target == null) return;

            if (target.TryGetComponent<CardData>(out var card) &&
                card.TryGetCardFeature<HealthDecorator>(out var health))
            {
                health.TakeDamage(currentDamageValue);
            }
        }

        public override void UpdateTarget(GameObject newTarget) => target = newTarget;
        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    #endregion
}