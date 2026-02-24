using Game.Cards.Strategy;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Ignore Shields modifier", menuName = "Modifiers/Ignore Shields")]
    public sealed class ModifierIgnoreShields : ModifierBase
    {
        public override void Apply(ModifierContext context)
        {
            context.SourceCard.Parent.GetComponent<CardData>()
                .GetCardFeature<AttackDecorator>()
                .ChangeStrategy(new IgnoreDefenceTypeAttackStategy(context.Player, DefenceType.Shield, context.DamageValue, context.DefendDeck));
        }
    }
}