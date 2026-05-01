using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierCorrosion", menuName = "Modifier/ModifierCorrosion", order = 0)]
    public sealed class ModifierCorrosion : ModifierBase
    {
        public override void OnCallBack(ModifierContext context)
        {            
            if (context.SourceCardData.TryGetCardFeature<HealthDecorator>(out var decorator))
            {
                var attackDecorator = context.TargetCardData.GetCardFeature<AttackDecorator>();
                int currentDamage = attackDecorator.currentDamage.Value;
                
                attackDecorator.ChangeDamage(currentDamage + currentDamage);
                decorator.TakeDamage(attackDecorator.currentDamage.Value);
                attackDecorator.ChangeDamage(currentDamage);

                if(context.SourceCardData.TryGetCardFeature<ModifierDecorator>(out var modifierDecorator)) modifierDecorator.RemoveModifier(this);
            }
        }
    }
}