using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierStunning", menuName = "Modifier/ModifierStunning", order = 0)]
    public sealed class ModifierStunning : ModifierBase
    {
        [SerializeField] private int CountStep;
        private int step;

        public override void Init(ModifierContext context)
        {
            step = CountStep;

            if(context.SourceCardData.TryGetCardFeature<StepCombatDecorator>(out var stepDecorator)) stepDecorator.Stop(true);
        }

        public override void OnGeneralUpdate(ModifierContext context)
        {
            Debug.Log(step);

            if(step > 0)
            {
                step--;
                return;
            }

            context.SourceCardData.GetCardFeature<ModifierDecorator>().RemoveModifier(this);
        }

        public override void OnRemove(ModifierContext context)
        {
            if(context.SourceCardData.TryGetCardFeature<StepCombatDecorator>(out var stepDecorator)) stepDecorator.Stop(false);
        }
    }
}