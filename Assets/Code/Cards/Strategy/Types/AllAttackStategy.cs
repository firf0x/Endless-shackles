using System;
using System.Runtime.CompilerServices;
using Game.Cards.Modifier;
using Game.Deck.Fabric;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    #region Standard Attack Strategies

    [Serializable]
    public class Standart : StrategyAttackBase
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
    public class Monster : StrategyAttackBase
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

    #region Monster Attack Strategies

    [Serializable]
    public class MonsterIgnoreDefenceType : StrategyAttackBase
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
    public class MonsterRandomIgnoreDefenceType : StrategyAttackBase
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
    public class MonsterDeckStrikeAttackRegenerate : StrategyAttackBase
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
    public class MonsterMultiplyDamage : StrategyAttackBase
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
    public class MonsterRandomMultiplyDamageByType : StrategyAttackBase
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
    public class MonsterCumulativeDamage : StrategyAttackBase
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
    public class MonsterDoubleStrikeWithCooldownReduction : StrategyAttackBase
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
    public class MonsterSummonOnHit : StrategyAttackBase
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
    public class MonsterFreezeRandomCard : StrategyAttackBase
    {
        public override string Name => "Freeze Random Card";
        // private CardData card;
        [Header("Здесь нужно указать модификатор на Stanning always")]
        [SerializeField] private ModifierBase modifier;
        private ModifierDecorator modifierDecorator;

        // В текщем случаем случае target мы используем только для нанесения урона.
        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            var deck = parent.GetComponent<CardData>().GetCardFeature<ModifierDecorator>().handDeck;
            
            if(modifierDecorator == null && deck.GetCardCount() > 0)
            {
                
                if(deck.cardDatas[UnityEngine.Random.Range(0, deck.GetCardCount())].TryGetCardFeature(out modifierDecorator)) 
                
                if(modifier != null)
                {
                    modifierDecorator.AddModifier(modifier);
                    modifierDecorator.Apply(target);
                }
            }

            if (target.GetComponent<CardData>().TryGetCardFeature<HealthDecorator>(out var decorator)) decorator.TakeDamage(damage);
        }

        public override void OnRemove()
        {
            if(modifierDecorator != null)
            {
                modifierDecorator.RemoveModifier(modifier);
                modifierDecorator = null;
            }
        }
    }

    #endregion

    #region Card Attack Strategies

    [Serializable]
    public class AttackOneshot : StrategyAttackBase
    {
        public override string Name => "One Shot";
        [SerializeField] private int Value;

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            if (target == null) return;

            var data = target.GetComponent<CardData>();
            if (data.TryGetCardFeature<HealthDecorator>(out var healthDecorator) && data.TryGetCardFeature<AttackDecorator>(out var attackDecorator))
            {
                // Удаление карты атаки при нанесении урона монстру
                if (data.decorateCard.Type.HasFlag(CardTypeEnum.Monster)) parent.GetComponent<CardData>().decorateCard.Destroy();

                // Если у врага урон меньше, чем у карты то мгновенно убивает врага
                if(attackDecorator.currentDamage.Value < damage) healthDecorator.TakeDamage(999);
                else healthDecorator.TakeDamage(damage);
                
                StepCombatSystem.Instance.StepUpdate();
            }
        }
    }

    [Serializable]
    public class AttackThreePronged : StrategyAttackBase
    {
        public override string Name => "Three Pronged Attack";
        [SerializeField] private int Value;

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            if (target == null) return;

            CardData data = target.GetComponent<CardData>();
            if (data.TryGetCardFeature<HealthDecorator>(out var healthDecorator) && data.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator))
            {
                CardData[] monsterCards = modifierDecorator.monsterDeck.cardDatas;

                // Удаление карты атаки при нанесении урона монстру
                if (data.decorateCard.Type.HasFlag(CardTypeEnum.Monster)) parent.GetComponent<CardData>().decorateCard.Destroy();

                if (modifierDecorator.monsterDeck.GetCardCount() > 1)
                {
                    int index = Array.IndexOf(monsterCards, data);
                    
                    // Центр + бока
                    if (index >= 1 && index < monsterCards.Length - 1) 
                    {
                        for (int i = index - 1; i <= index + 1; i++)
                        {
                            CardData targetCard = monsterCards[i];
                            if (targetCard != null && targetCard.TryGetCardFeature<HealthDecorator>(out var health)) health.TakeDamage(damage);
                        }
                    }
                    else if (index > 0) // Центр + левая
                    {
                        for (int i = index - 1; i <= index; i++)
                        {
                            CardData targetCard = monsterCards[i];
                            if (targetCard != null && targetCard.TryGetCardFeature<HealthDecorator>(out var health)) health.TakeDamage(damage);
                        }
                    }
                    else if (index < monsterCards.Length - 1) // Центр + правая
                    {
                        for (int i = index; i <= index + 1; i++)
                        {
                            CardData targetCard = monsterCards[i];
                            if (targetCard != null && targetCard.TryGetCardFeature<HealthDecorator>(out var health)) health.TakeDamage(damage);
                        }
                    }
                    else healthDecorator.TakeDamage(damage);
                }
                else healthDecorator.TakeDamage(damage);


                StepCombatSystem.Instance.StepUpdate();
            }
        }
    }

    [Serializable]
    public class AttackSetEnemyModifier : StrategyAttackBase
    {
        public override string Name => "Set Enemy Modifier";
        [SerializeField] private ModifierBase modifier;

        public override void Execute(GameObject parent, GameObject target, int damage)
        {
            if (target == null) return;

            var data = target.GetComponent<CardData>();
            if (data.TryGetCardFeature<HealthDecorator>(out var healthDecorator) && data.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator))
            {
                // Удаление карты атаки при нанесении урона монстру
                if (data.decorateCard.Type.HasFlag(CardTypeEnum.Monster)) parent.GetComponent<CardData>().decorateCard.Destroy();

                healthDecorator.TakeDamage(damage);

                StepCombatSystem.Instance.StepUpdate();

                // Добавление модификатора на карту монстра                
                modifierDecorator.AddModifier(modifier);
            }
        }
    }

    #endregion
}