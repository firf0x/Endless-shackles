using Game.Cards.Strategy;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Multiply damage modifier", menuName = "Modifiers/Multiply damage")]
    public sealed class ModifierMultiplyDamage : ModifierBase
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
                .ChangeStrategy(new MultiplyDamageStrategy(context.Player, context.DamageValue, context.DefendDeck, context.SourceGameObject.GetComponent<CardData>().GetCardFeature<AttackDecorator>()));
            
            isActive = false;
        }
    }
}