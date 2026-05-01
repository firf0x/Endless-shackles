using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierFear", menuName = "Modifier/ModifierFear", order = 0)]
    public sealed class ModifierFear : ModifierBase
    {
        public override void OnGeneralUpdate(ModifierContext context)
        {
            context.SourceCardData.GetCardFeature<HealthDecorator>().isTarget = false;
        }

        public override void OnRemove(ModifierContext context)
        {
            context.SourceCardData.GetCardFeature<HealthDecorator>().isTarget = true;
        }
    }
}