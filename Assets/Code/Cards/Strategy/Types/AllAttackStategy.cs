using System;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    #region Standard Attack Strategies

    [Serializable]
    public class StandartAttackStategy : StrategyAttackBase
    {
        public override string Name => "Default Attack";

        [HideInInspector] public GameObject parent;
        [HideInInspector] public int currentDamageValue;

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            if (target == null) return;

            var data = target.GetComponent<CardData>();
            if (data.TryGetCardFeature<HealthDecorator>(out var feature))
            {
                // Удаление карты атаки при нанесении урона монстру
                if (data.decorateCard.Type.HasFlag(CardTypeEnum.Monster)) parent.GetComponent<CardData>().decorateCard.Destroy();

                feature.TakeDamage(currentDamageValue);
                StepCombatSystem.Instance.StepUpdate();
            }
        }
    }

    public class MonsterAttackStrategy : StrategyAttackBase
    {
        public override string Name => "Monster Attack";

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            if (target == null)
            {
                PlayerSystem.Instance.Kill();
                return;
            }

            if (target.TryGetComponent<CardData>(out var card) && card.TryGetCardFeature<HealthDecorator>(out var health))
            {
                health.TakeDamage(damage);
            }
        }
    }

    #endregion

    #region Modifier Attack Strategies

    [Serializable]
    public class IgnoreDefenceTypeAttackStategy : StrategyAttackBase
    {
        public override string Name => "Ignore defence type attack";

        [SerializeField] private DefenceType type;

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            CardData defenceItem = null;
            var cardData = target.GetComponent<CardData>();

            if(cardData.currentDeck.GetCardCount() == 0)
            {
                PlayerSystem.Instance.Kill();
                return;
            }

            foreach (var card in cardData.currentDeck.cardDatas)
            {
                if(card == null) continue;

                if (card.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType != type)
                {
                    card.GetCardFeature<HealthDecorator>().TakeDamage(damage);
                    return;
                }
                else defenceItem = card;
            }

            if(defenceItem != null) PlayerSystem.Instance.Kill();
        }
    }

    [Serializable]
    public class RandomIgnoreDefenceTypeAttackStategy : StrategyAttackBase
    {
        public override string Name => "Random ignore defence type attack";
        
        private DefenceType type;

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            type = (DefenceType)UnityEngine.Random.Range(0, (int)DefenceType.Shield);

            CardData defenceItem = null;
            var cardData = target.GetComponent<CardData>();

            if(cardData.currentDeck.GetCardCount() == 0)
            {
                PlayerSystem.Instance.Kill();
                return;
            }

            foreach (var card in cardData.currentDeck.cardDatas)
            {
                if(card == null) continue;

                if (card.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType != type)
                {
                    card.GetCardFeature<HealthDecorator>().TakeDamage(damage);
                    return;
                }
                else defenceItem = card;
            }

            if(defenceItem != null) PlayerSystem.Instance.Kill();
        }
    }

    // [Serializable]
    // public class DeckStrikeAttackRegenerateStrategy : StrategyAttackBase
    // {
    //     public override string Name => "Deck strike";
    //     // private PlayerSystem player;
    //     // private int currentDamageValue;
    //     // private int currentHealValue;
    //     // private IDeck<CardData> monsterDeck;
    //     // private ICard<CardTypeEnum> objectCard;
    //     // private GameObject target;

    //     public override void Execute()
    //     {
    //         // var cardData = target.GetComponent<CardData>();

    //         // if(cardData.currentDeck.GetCardCount() > 0 )
    //         // {
    //         //     int multiply = 0;

    //         //     foreach (var card in cardData.currentDeck.cardDatas)
    //         //     {
    //         //         if(card == null || card.decorateCard == null) continue;
    //         //         if(card.TryGetCardFeature<HealthDecorator>(out var decorator))
    //         //         {
    //         //             multiply++;
    //         //             decorator.TakeDamage(currentDamageValue);
    //         //             break;
    //         //         }
    //         //     }

    //         //     foreach (var card in monsterDeck.cardDatas)
    //         //     {
    //         //         if(card == null || objectCard.Parent == card.gameObject || !card.TryGetCardFeature<HealthDecorator>(out var decorator)) continue;
    //         //         decorator.Heal(currentHealValue * multiply);
    //         //     }
    //         // }
    //         // else player.Kill();
    //     }

    //     // public override void UpdateTarget(GameObject newTarget) => target = newTarget;
    //     // public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    // }

    // [Serializable]
    // public class MultiplyDamageStrategy : StrategyAttackBase
    // {
    //     public override string Name => "Multiply damage Attack";
    //     // private PlayerSystem player;
    //     // private AttackDecorator currentCard;
    //     // private GameObject target;

    //     public override void Execute()
    //     {
    //         // if (target == null)
    //         // {
    //         //     player.Kill();
    //         //     return;
    //         // }

    //         // if (target.GetComponent<CardData>().TryGetCardFeature<HealthDecorator>(out var decorator)) decorator.TakeDamage(currentCard.currentDamage.Value * 2);
    //     }

    //     // public override void UpdateTarget(GameObject newTarget) => target = newTarget;
    // }

    // [Serializable]
    // public class MultiplyDamageByTypeStrategy : StrategyAttackBase
    // {
    //     public override string Name => "Multiply by type Attack";
    //     // private PlayerSystem player;
    //     // private int currentDamageValue;
    //     // private DefenceType type;
    //     // private GameObject target;

    //     public override void Execute()
    //     {
    //         // if (target == null)
    //         // {
    //         //     player.Kill();
    //         //     return;
    //         // }

    //         // var cardData = target.GetComponent<CardData>();

    //         // if (!cardData.TryGetCardFeature<HealthDecorator>(out var health)) return;

    //         // int damage = currentDamageValue;
    //         // var defenceType = cardData.GetCardFeature<CustomTypeDecorator<DefenceType>>();

    //         // if (defenceType != null && defenceType.CustomType == type) damage *= 2;

    //         // health.TakeDamage(damage);
    //     }

    //     // public override void UpdateTarget(GameObject newTarget) => target = newTarget;
    //     // public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    // }

    // // Это что за пиздец?
    // [Serializable]
    // public class LoopAttackByStepStrategy : StrategyAttackBase
    // {
    //     public override string Name => "Loop by step Attack";
    //     // private int currentDamageValue;
    //     // private GameObject target;

    //     public override void Execute()
    //     {
    //         // if (target == null) return;

    //         // if (target.TryGetComponent<CardData>(out var card) &&
    //         //     card.TryGetCardFeature<HealthDecorator>(out var health))
    //         // {
    //         //     health.TakeDamage(currentDamageValue);
    //         // }
    //     }

    //     // public override void UpdateTarget(GameObject newTarget) => target = newTarget;
    //     // public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    // }

    // [Serializable]
    // public class ExponentialGrowthAttackStrategy : StrategyAttackBase
    // {
    //     public override string Name => "Exponential Growth Attack Strategy";
    //     // private int currentDamageValue;
    //     // private int index;
    //     // private GameObject target;

    //     public override void Execute()
    //     {
    //         // if (target == null) return;

    //         // if (target.TryGetComponent<CardData>(out var card) && card.TryGetCardFeature<HealthDecorator>(out var health))
    //         // {
    //         //     health.TakeDamage(currentDamageValue * index);
    //         //     index++;
    //         // }
    //     }

    //     // public override void UpdateTarget(GameObject newTarget) => target = newTarget;
    //     // public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    // }

    #endregion
}