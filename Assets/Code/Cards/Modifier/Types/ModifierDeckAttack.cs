using Game.Cards.Strategy;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "deck strike modifier", menuName = "Modifiers/Deck Strike")]
    public sealed class ModifierDeckStrike : ModifierBase
    {
        [SerializeField] private int RegenerateHP;
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
                .ChangeStrategy(new DeckStrikeAttackRegenerateStrategy(context.Player, context.DamageValue, RegenerateHP, context.MonsterDeck, context.SourceCard));
        
            isActive = false;
        }
    }
}