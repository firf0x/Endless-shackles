using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Stunning modifier", menuName = "Modifiers/Stunning")]
    public sealed class ModifierStunning : ModifierBase
    {
        [SerializeField] private int CountStep;
        private int step;

        public override void Init(ModifierContext context)
        {
            step = CountStep;
            context.SourceCard.SetLock(!context.SourceCard.isLocked);
        }

        public override void OnUpdate(ModifierContext context)
        {
            if(step > 0)
            {
                step--;
                return;
            }

            context.SourceCard.SetLock(!context.SourceCard.isLocked);
            context.SourceCardData.GetCardFeature<ModifierDecorator>().RemoveModifier(this);
        }
    }
}