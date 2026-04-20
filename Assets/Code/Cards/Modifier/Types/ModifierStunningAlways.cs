using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Stunning Always modifier", menuName = "Modifiers/Stunning Always")]
    public sealed class ModifierStunningAlways : ModifierBase
    {
        // Примечание: Этот эффект нужен для постоянного стана карты до удаления самого модификатора
        public override void Apply(ModifierContext context)
        {
            context.SourceCard.SetLock(true);
        }


        public override void OnRemove(ModifierContext context) => context.SourceCard.SetLock(false);
    }
}