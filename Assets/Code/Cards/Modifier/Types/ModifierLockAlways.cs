using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "ModifierLockAlways", menuName = "Modifier/ModifierLockAlways", order = 0)]
    public sealed class ModifierLockAlways : ModifierBase
    {
        // Примечание: Этот эффект нужен для постоянного стана карты в руке до удаления самого модификатора
        public override void OnGeneralUpdate(ModifierContext context)
        {
            context.SourceCard.SetLock(true);
        }


        public override void OnRemove(ModifierContext context) => context.SourceCard.SetLock(false);
    }
}