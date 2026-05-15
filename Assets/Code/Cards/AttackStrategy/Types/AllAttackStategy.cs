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
    public class Standart : AttackStrategyBase
    {
        public override string Name => "Default Attack";

        public override void Execute(CombatArgs args)
        {
            var targetData = args.Target;
            if (targetData.TryGetCardFeature<HealthDecorator>(out var feature))
            {
                if (targetData.decorateCard.Type.HasFlag(CardTypeEnum.Monster)) args.Parent.decorateCard.Destroy(); // TODO: замена
                
                // Удаление карты атаки при нанесении урона монстру
                feature.TakeDamage(args.Damage);
                StepCombatSystem.Instance.StepUpdate();
            }
        }
    }

    [Serializable]
    public class Monster : AttackStrategyBase
    {
        public override string Name => "Monster Attack";

        public override void Execute(CombatArgs args)
        {
            if (args.Target.TryGetCardFeature<HealthDecorator>(out var health))
            {
                health.TakeDamage(args.Damage);
            }
        }
    }

    #endregion

    #region Monster Attack Strategies

    [Serializable]
    public class MonsterIgnoreDefenceType : AttackStrategyBase
    {
        public override string Name => "Ignore defence type attack";

        [SerializeField] private DefenceType type;

        public override void Execute(CombatArgs args)
        {
            CardData defenceItem = null;
            var cardData = args.Target;

            foreach (var card in cardData.currentDeck.cardDatas)
            {
                if (card == null) continue;

                if (card.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType != type)
                {
                    card.GetCardFeature<HealthDecorator>().TakeDamage(args.Damage);
                    return;
                }
                else defenceItem = card;
            }

            if (defenceItem != null) PlayerSystem.Instance.Kill();
        }
    }

    [Serializable]
    public class MonsterRandomIgnoreDefenceType : AttackStrategyBase
    {
        public override string Name => "Random ignore defence type";
        
        private DefenceType type;

        public override void Init()
        {
            type = (DefenceType)UnityEngine.Random.Range(0, (int)DefenceType.Shield);            
        }

        public override void Execute(CombatArgs args)
        {
            CardData defenceItem = null;
            var cardData = args.Target;

            foreach (var card in cardData.currentDeck.cardDatas)
            {
                if (card == null) continue;

                if (card.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType != type)
                {
                    card.GetCardFeature<HealthDecorator>().TakeDamage(args.Damage);
                    return;
                }
                else defenceItem = card;
            }

            if (defenceItem != null) PlayerSystem.Instance.Kill();
        }
    }

    [Serializable]
    public class MonsterDeckStrikeAttackRegenerate : AttackStrategyBase
    {
        public override string Name => "Deck strike";
        [SerializeField] private int currentHealValue;

        public override void Execute(CombatArgs args)
        {
            var cardData = args.Target;

            if (cardData.currentDeck.GetCardCount() > 0)
            {
                int multiply = 0;

                foreach (var card in cardData.currentDeck.cardDatas)
                {
                    if (card == null || card.decorateCard == null) continue;
                    if (card.TryGetCardFeature<HealthDecorator>(out var decorator))
                    {
                        multiply++;
                        decorator.TakeDamage(args.Damage);
                        break;
                    }
                }

                foreach (var card in args.Parent.currentDeck.cardDatas)
                {
                    if (card == null || args.Parent == card || !card.TryGetCardFeature<HealthDecorator>(out var decorator)) continue;
                    decorator.Heal(currentHealValue * multiply);
                }
            }
        }
    }

    [Serializable]
    public class MonsterMultiplyDamage : AttackStrategyBase
    {
        public override string Name => "Multiply damage Attack";
        [SerializeField] private int Multiply = 2;

        public override void Execute(CombatArgs args)
        {
            if (args.Target.TryGetCardFeature<HealthDecorator>(out var decorator))
                decorator.TakeDamage(args.Damage * Multiply);
        }
    }

    [Serializable]
    public class MonsterRandomMultiplyDamageByType : AttackStrategyBase
    {
        public override string Name => "Random multiply by type Attack";
        [SerializeField] private int Multiply = 2;
        private DefenceType type;

        public override void Init() => type = (DefenceType)UnityEngine.Random.Range(0, (int)DefenceType.Shield);

        public override void Execute(CombatArgs args)
        {
            var cardData = args.Target;

            if (!cardData.TryGetCardFeature<HealthDecorator>(out var health)) return;

            var defenceType = cardData.GetCardFeature<CustomTypeDecorator<DefenceType>>();

            int newDamage = args.Damage;
            if (defenceType != null && defenceType.CustomType == type)
            {
                newDamage *= Multiply;
            }

            health.TakeDamage(newDamage);
        }
    }

    [Serializable]
    public class MonsterCumulativeDamage : AttackStrategyBase
    {
        public override string Name => "Loop by step Attack";

        public override void Execute(CombatArgs args)
        {
            if (args.Target.TryGetCardFeature<HealthDecorator>(out var health))
            {
                health.TakeDamage(args.Damage);
                args.Parent.GetCardFeature<AttackDecorator>().ChangeDamage(args.Damage * 2);
            }
        }
    }

    [Serializable]
    public class MonsterDoubleStrikeWithCooldownReduction : AttackStrategyBase
    {
        public override string Name => "Double Strike With Cooldown Reduction";
        [SerializeField] private int count = 1;

        public override void Execute(CombatArgs args)
        {
            var target = args.Target;
            
            for (int i = 0; i < count; i++)
            {
                if (target.TryGetCardFeature<HealthDecorator>(out var health))
                    health.TakeDamage(args.Damage);
            }

            if (target.TryGetCardFeature<StepCombatDecorator>(out var decorator))
                decorator.OnUpdate();
        }
    }

    [Serializable]
    public class MonsterSummonOnHit : AttackStrategyBase
    {
        public override string Name => "Summon Card On Hit";
        [SerializeField] private MinionsCreator creator = new MinionsCreator();

        public override void Execute(CombatArgs args)
        {
            var cardInfo = args.Parent.GetCardFeature<ModifierDecorator>();

            creator.handDeck = cardInfo.handDeck;
            creator.defenceDeck = cardInfo.defendDeck;
            creator.monsterDeck = cardInfo.monsterDeck;

            creator.Create();

            if (args.Target.TryGetCardFeature<HealthDecorator>(out var decorator))
                decorator.TakeDamage(args.Damage);
        }
    }

    [Serializable]
    public class MonsterFreezeRandomCard : AttackStrategyBase
    {
        public override string Name => "Freeze Random Card";

        [Header("Здесь нужно указать модификатор на Stanning always")]
        [SerializeField] private ModifierBase modifier;
        private ModifierDecorator modifierDecorator;

        public override void Execute(CombatArgs args)
        {
            var deck = args.Parent.GetCardFeature<ModifierDecorator>().handDeck;
            
            if (modifierDecorator == null && deck.GetCardCount() > 0)
            {
                if (deck.cardDatas[UnityEngine.Random.Range(0, deck.GetCardCount())].TryGetCardFeature(out modifierDecorator))
                {
                    if (modifier != null)
                    {
                        modifierDecorator.AddModifier(modifier);
                    }
                }
            }

            if (args.Target.TryGetCardFeature<HealthDecorator>(out var decorator))
                decorator.TakeDamage(args.Damage);
        }

        public override void OnRemove()
        {
            if (modifierDecorator != null)
            {
                modifierDecorator.RemoveModifier(modifier);
                modifierDecorator = null;
            }
        }
    }

    [Serializable]
    public class MonsterRepeatedDamage : AttackStrategyBase
    {
        public override string Name => "Random repeated damage";
        [SerializeField] private int Repeat;

        public override void Execute(CombatArgs args)
        {
            for (int i = 0; i < Repeat; i++)
            {
                args.Target.GetCardFeature<HealthDecorator>().TakeDamage(args.Damage);
            }
        }
    }

    [Serializable]
    public class MonsterDestroyerCardInHand : AttackStrategyBase
    {
        public override string Name => "Destroyer card in hand";
        [SerializeField] private int MaxCountOnDestroy;

        public override void Execute(CombatArgs args)
        {
            var cardsHand = args.Parent.GetCardFeature<ModifierDecorator>().handDeck;

            int lengthDestroy = UnityEngine.Random.Range(0, MaxCountOnDestroy);

            for (int i = 0; i < lengthDestroy; i++)
            {
                if(cardsHand.GetCardCount() < 0) break;

                CardData card = cardsHand.cardDatas[UnityEngine.Random.Range(0, cardsHand.GetCardCount())];
            
                if(card != null) card.CardDestroy(true);
            }

            args.Target.GetCardFeature<HealthDecorator>().TakeDamage(args.Damage);
        }
    }


    [Serializable]
    public class MonsterSetYourselfModifier : AttackStrategyBase
    {
        public override string Name => "Set Yourself Modifier";
        [SerializeField] private ModifierBase modifier;

        public override void Execute(CombatArgs args)
        {
            if (args.Target == null) return;

            var healthDecorator = args.Target.GetCardFeature<HealthDecorator>();
            if (args.Parent.TryGetCardFeature<AttackDecorator>(out var attackDecorator) && args.Parent.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator))
            {
                healthDecorator.TakeDamage(args.Damage);

                // StepCombatSystem.Instance.StepUpdate();

                // Добавление модификатора на карту монстра
                modifierDecorator.AddModifier(modifier);
            }
        }
    }

    #endregion

    #region Card Attack Strategies

    [Serializable]
    public class AttackOneshot : AttackStrategyBase
    {
        public override string Name => "One Shot";
        [SerializeField] private int Value;

        public override void Execute(CombatArgs args)
        {
            if (args.Target == null) return;

            var targetData = args.Target;
            if (targetData.TryGetCardFeature<HealthDecorator>(out var healthDecorator) &&
                targetData.TryGetCardFeature<AttackDecorator>(out var attackDecorator))
            {
                // Удаление карты атаки при нанесении урона монстру
                if (targetData.decorateCard.Type.HasFlag(CardTypeEnum.Monster))
                    args.Parent.decorateCard.Destroy();

                // Если у врага урон меньше, чем у карты то мгновенно убивает врага
                if (attackDecorator.currentDamage.Value < args.Damage)
                    healthDecorator.TakeDamage(999);
                else
                    healthDecorator.TakeDamage(args.Damage);
                
                StepCombatSystem.Instance.StepUpdate();
            }
        }
    }

    [Serializable]
    public class AttackThreePronged : AttackStrategyBase
    {
        public override string Name => "Three Pronged Attack";
        [SerializeField] private int Value;

        public override void Execute(CombatArgs args)
        {
            if (args.Target == null) return;

            var targetData = args.Target;
            if (targetData.TryGetCardFeature<HealthDecorator>(out var healthDecorator) &&
                targetData.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator))
            {
                CardData[] monsterCards = modifierDecorator.monsterDeck.cardDatas;

                // Удаление карты атаки при нанесении урона монстру
                if (targetData.decorateCard.Type.HasFlag(CardTypeEnum.Monster))
                    args.Parent.decorateCard.Destroy();

                if (modifierDecorator.monsterDeck.GetCardCount() > 1)
                {
                    int index = Array.IndexOf(monsterCards, targetData);
                    
                    // Центр + бока
                    if (index >= 1 && index < monsterCards.Length - 1)
                    {
                        for (int i = index - 1; i <= index + 1; i++)
                        {
                            CardData targetCard = monsterCards[i];
                            if (targetCard != null && targetCard.TryGetCardFeature<HealthDecorator>(out var health))
                                health.TakeDamage(args.Damage);
                        }
                    }
                    else if (index > 0) // Центр + левая
                    {
                        for (int i = index - 1; i <= index; i++)
                        {
                            CardData targetCard = monsterCards[i];
                            if (targetCard != null && targetCard.TryGetCardFeature<HealthDecorator>(out var health))
                                health.TakeDamage(args.Damage);
                        }
                    }
                    else if (index < monsterCards.Length - 1) // Центр + правая
                    {
                        for (int i = index; i <= index + 1; i++)
                        {
                            CardData targetCard = monsterCards[i];
                            if (targetCard != null && targetCard.TryGetCardFeature<HealthDecorator>(out var health))
                                health.TakeDamage(args.Damage);
                        }
                    }
                    else healthDecorator.TakeDamage(args.Damage);
                }
                else healthDecorator.TakeDamage(args.Damage);

                StepCombatSystem.Instance.StepUpdate();
            }
        }
    }

    [Serializable]
    public class AttackSetEnemyModifier : AttackStrategyBase
    {
        public override string Name => "Set Enemy Modifier";
        [SerializeField] private ModifierBase modifier;

        public override void Execute(CombatArgs args)
        {
            if (args.Target == null) return;

            var targetData = args.Target;
            if (targetData.TryGetCardFeature<HealthDecorator>(out var healthDecorator) && targetData.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator))
            {
                if (targetData.decorateCard.Type.HasFlag(CardTypeEnum.Monster)) args.Parent.decorateCard.Destroy();

                healthDecorator.TakeDamage(args.Damage);

                StepCombatSystem.Instance.StepUpdate();

                // Добавление модификатора на карту монстра
                modifierDecorator.AddModifier(modifier);
            }
        }
    }

    #endregion
}