using Game.Cards.Strategy;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Loop attack by step modifier", menuName = "Modifiers/LoopAttackByStep")]
    public sealed class ModifierLoopAttackByStep : ModifierBase
    {
        private bool isActive;

        public override void Init()
        {
            isActive = true;
        }

        public override void Apply(ModifierContext context)
        {
            if(isActive == false) return;

            context.SourceCard.Parent.GetComponent<CardData>()
                .GetCardFeature<AttackDecorator>()
                .ChangeStrategy(new LoopAttackByStepStrategy(context.DamageValue));

            isActive = false;
        }
    }
}