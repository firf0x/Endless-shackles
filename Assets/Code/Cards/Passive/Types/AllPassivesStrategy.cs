using System;
using System.Collections.Generic;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Passive
{
    #region Monsters
    
    [Serializable]
    public sealed class MonsterHpToAttackAndCoolDown : PassivesBase
    {
        [SerializeField] private int multiplicity;

        private HealthDecorator healthDecorator;
        private AttackDecorator attackDecorator;
        private StepCombatDecorator stepDecorator;

        public override void Init(PassiveContext context)
        {
            healthDecorator = context.Parent.GetComponent<CardData>().GetCardFeature<HealthDecorator>();
            attackDecorator = context.Parent.GetComponent<CardData>().GetCardFeature<AttackDecorator>();
            stepDecorator = context.Parent.GetComponent<CardData>().GetCardFeature<StepCombatDecorator>();
        }

        public override void OnUpdate(PassiveContext context) => UpdateStats(healthDecorator.healthSystem.HealPoints.Value);

        public void UpdateStats(int hp)
        {
            int bonus = hp / multiplicity;

            attackDecorator.ChangeDamage(bonus);
            stepDecorator.ChangeLimits(bonus);
        }
    }

    [Serializable]
    public sealed class MonsterRiseDamageAndRandomCooldown : PassivesBase
    {
        [SerializeField] private int DamageBuf;

        public override void OnUpdate(PassiveContext context)
        {
            if(context.MonsterDeck.GetCardCount() <= 0) return;

            foreach (var card in context.MonsterDeck.cardDatas)
            {
                if(card == null || context.Parent == card.gameObject) continue;
                else
                {
                    var decorator = card.GetCardFeature<AttackDecorator>(); 
                    decorator.ChangeDamage(decorator.defaultDamageValue + DamageBuf);
                }
            }

            CardData randomCard = context.MonsterDeck.cardDatas[UnityEngine.Random.Range(0, context.MonsterDeck.GetCardCount())];
            randomCard.GetCardFeature<StepCombatDecorator>().OnUpdate();
        }
    }

    [Serializable]
    public sealed class MonsterRetaliatoryStrike : PassivesBase
    {
        [SerializeField] private ModifierBase IgnoringStun;

        public override void OnCallBack(PassiveContext context)
        {
            if(context.Target.GetComponent<CardData>().TryGetCardFeature<HealthDecorator>(out var healthDecorator))
            {
                if (context.Parent.GetComponent<CardData>().TryGetCardFeature<AttackDecorator>(out var decorator)) healthDecorator.TakeDamage(decorator.currentDamage.Value);
            }
        }

        public override void OnRemove(PassiveContext context)
        {
            if(context.Parent.GetComponent<CardData>().TryGetCardFeature<ModifierDecorator>(out var modifierDecorator) && context.Parent.GetComponent<CardData>().TryGetCardFeature<AttackDecorator>(out var decorator))
            {
                if(!modifierDecorator.HasModifier(IgnoringStun))
                {
                    context.DefenceDeck.cardDatas[UnityEngine.Random.Range(0, context.DefenceDeck.GetCardCount())]
                        .GetCardFeature<HealthDecorator>()
                        .TakeDamage(decorator.currentDamage.Value);
                }
            }
        }

    }

    [Serializable]
    public sealed class MonsterTakeNegativeModifier : PassivesBase
    {
        [SerializeField] private List<ModifierBase> NegativeModifiers;

        public override void OnCallBack(PassiveContext context)
        {
            if(NegativeModifiers.Count > 0)
            {
                ModifierBase resModifier = NegativeModifiers[UnityEngine.Random.Range(0, NegativeModifiers.Count)];
                context.DefenceDeck.cardDatas[UnityEngine.Random.Range(0, context.DefenceDeck.GetCardCount())].GetCardFeature<ModifierDecorator>().AddModifier(resModifier);
            }
        }
    }

    #endregion

    #region Other

    [Serializable]
    public sealed class RemoveAlwaysCertainModifier : PassivesBase
    {
        [SerializeField] private ModifierBase selectedModifier;

        public override void Init(PassiveContext context)
        {
            var cardData = context.Parent.GetComponent<CardData>();

            if (cardData.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator) && modifierDecorator.HasModifier(selectedModifier)) modifierDecorator.RemoveModifier(selectedModifier);
        }

        public override void OnGeneralUpdate(PassiveContext context)
        {
            var cardData = context.Parent.GetComponent<CardData>();
            
            if (cardData.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator) && modifierDecorator.HasModifier(selectedModifier)) modifierDecorator.RemoveModifier(selectedModifier);
        }
    }

    #endregion
}