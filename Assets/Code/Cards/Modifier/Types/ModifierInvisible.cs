using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Invisible modifier", menuName = "Modifiers/Invisible")]
    public sealed class ModifierInvisible : ModifierBase
    {
        public override void Apply(ModifierContext context) { }
        public override void OnUpdate(ModifierContext context)
        {
            Debug.Log($"Invisible : {context.SourceCard.isLocked}");
            context.SourceCard.SetLock(!context.SourceCard.isLocked);
        }
    }
}