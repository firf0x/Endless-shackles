using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierPoisoning", menuName = "Modifier/ModifierPoisoning", order = 0)]
    public sealed class ModifierPoisoning : ModifierBase
    {
        private int maxDamage = 0;
        public override void OnCallBack(ModifierContext context)
        {
            int damage = context.TargetCardData.GetCardFeature<AttackDecorator>().currentDamage.Value;
            
            if(damage > context.SourceCardData.GetCardFeature<HealthDecorator>().healthSystem.HealPoints.Value) return;

            float currentDamage = damage / 2;

            if(maxDamage < currentDamage) maxDamage = Mathf.CeilToInt(currentDamage);
            if(maxDamage <= 0) maxDamage = 1;

            context.SourceCardData.GetCardFeature<HealthDecorator>().TakeDamage(maxDamage);
        }

    }
}