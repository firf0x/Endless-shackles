using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierInvisible", menuName = "Modifier/ModifierInvisible", order = 0)]
    public sealed class ModifierInvisible : ModifierBase
    {
        public override void OnUpdate(ModifierContext context)
        {
            context.SourceCard.SetLock(!context.SourceCard.isLocked);
        }
    }
}