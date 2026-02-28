using Game.Cards.Strategy;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "deck strike modifier", menuName = "Modifiers/Deck Strike")]
    public sealed class ModifierDeckStrike : ModifierBase
    {
        [SerializeField] private int RegenerateHP;
        public override void Apply(ModifierContext context)
        {
            context.SourceCard.Parent.GetComponent<CardData>()
                .GetCardFeature<AttackDecorator>()
                .ChangeStrategy(new DeckStrikeAttackRegenerateStrategy(context.Player, context.DamageValue, RegenerateHP, context.DefendDeck, context.MonsterDeck, context.SourceCard));
        }
    }
}