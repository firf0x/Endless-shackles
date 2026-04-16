using System;
using System.Runtime.CompilerServices;
using Game.Deck.Fabric;
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


        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            if (target == null) return;

            var data = target.GetComponent<CardData>();
            if (data.TryGetCardFeature<HealthDecorator>(out var feature))
            {
                // Удаление карты атаки при нанесении урона монстру
                if (data.decorateCard.Type.HasFlag(CardTypeEnum.Monster)) parent.GetComponent<CardData>().decorateCard.Destroy();

                feature.TakeDamage(damage);
                StepCombatSystem.Instance.StepUpdate();
            }
        }
    }

    [Serializable]
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
        public override string Name => "Random ignore defence type";
        
        private DefenceType type;

        public override void Init()
        {
            type = (DefenceType)UnityEngine.Random.Range(0, (int)DefenceType.Shield);            
        }

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
    public class DeckStrikeAttackRegenerateStrategy : StrategyAttackBase
    {
        public override string Name => "Deck strike";
        [SerializeField] private int currentHealValue;

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            var cardData = target.GetComponent<CardData>();

            if(cardData.currentDeck.GetCardCount() > 0 )
            {
                int multiply = 0;

                foreach (var card in cardData.currentDeck.cardDatas)
                {
                    if(card == null || card.decorateCard == null) continue;
                    if(card.TryGetCardFeature<HealthDecorator>(out var decorator))
                    {
                        multiply++;
                        decorator.TakeDamage(damage);
                        break;
                    }
                }

                foreach (var card in parent.GetComponent<CardData>().currentDeck.cardDatas)
                {
                    if(card == null || parent == card.gameObject || !card.TryGetCardFeature<HealthDecorator>(out var decorator)) continue;
                    decorator.Heal(currentHealValue * multiply);
                }
            }
            else PlayerSystem.Instance.Kill();
        }
    }

    [Serializable]
    public class MultiplyDamageStrategy : StrategyAttackBase
    {
        public override string Name => "Multiply damage Attack";
        [SerializeField] private int Multiply = 2;

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            if (target == null)
            {
                PlayerSystem.Instance.Kill();
                return;
            }

            if (target.GetComponent<CardData>().TryGetCardFeature<HealthDecorator>(out var decorator)) decorator.TakeDamage(damage * Multiply);
        }
    }

    [Serializable]
    public class RandomMultiplyDamageByTypeStrategy : StrategyAttackBase
    {
        public override string Name => "Random multiply by type Attack";
        [SerializeField] private int Multiply = 2;
        private DefenceType type;

        public override void Init() => type = (DefenceType)UnityEngine.Random.Range(0, (int)DefenceType.Shield);

        public override void Execute(GameObject parent, GameObject target, int damage)
        {   
            if (target == null)
            {
                PlayerSystem.Instance.Kill();
                return;
            }

            Debug.Log(type.ToString());

            var cardData = target.GetComponent<CardData>();

            if (!cardData.TryGetCardFeature<HealthDecorator>(out var health)) return;

            var defenceType = cardData.GetCardFeature<CustomTypeDecorator<DefenceType>>();

            int newDamage = damage;
            if (defenceType != null && defenceType.CustomType == type)
            {
                newDamage *= Multiply;
            }

            health.TakeDamage(newDamage);
        }
    }

    [Serializable]
    public class CumulativeDamageStrategy : StrategyAttackBase
    {
        public override string Name => "Loop by step Attack";

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            if (target == null) return;

            if (target.TryGetComponent<CardData>(out var card) && card.TryGetCardFeature<HealthDecorator>(out var health))
            {
                health.TakeDamage(damage);
                parent.GetComponent<CardData>().GetCardFeature<AttackDecorator>().ChangeDamage(damage * 2);
            }
        }
    }

    [Serializable]
    public class DoubleStrikeWithCooldownReductionStrategy : StrategyAttackBase
    {
        public override string Name => "Double Strike With Cooldown Reduction";
        [SerializeField] private int count = 1;


        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            target.TryGetComponent<CardData>(out var card);
            
            for (int i = 0; i < count; i++)
            {
                if (card.TryGetCardFeature<HealthDecorator>(out var health)) health.TakeDamage(damage);
            }

            if(card.TryGetCardFeature<StepCombatDecorator>(out var decorator)) decorator.OnUpdate();            
        }
    }

    [Serializable]
    public class SummonOnHitStrategy : StrategyAttackBase
    {
        public override string Name => "Summon Card On Hit";
        [SerializeField] private MinionsCreator creator = new MinionsCreator();

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            //TODO: Говно-код, это связано с тем что в init я не могу указать данные
            var cardInfo = parent.GetComponent<CardData>().GetCardFeature<ModifierDecorator>();

            creator.handDeck = cardInfo.handDeck;
            creator.defenceDeck = cardInfo.defendDeck;
            creator.monsterDeck = cardInfo.monsterDeck;

            creator.Create();

            if (target.GetComponent<CardData>().TryGetCardFeature<HealthDecorator>(out var decorator)) decorator.TakeDamage(damage);
        }
    }

    [Serializable]
    public class FreezeRandomCardStrategy : StrategyAttackBase
    {
        public override string Name => "Freeze Random Card";

        public override void Execute(GameObject parent, GameObject target, int damage)
        {

            if (target.GetComponent<CardData>().TryGetCardFeature<HealthDecorator>(out var decorator)) decorator.TakeDamage(damage);
        }
    }

    #endregion
}