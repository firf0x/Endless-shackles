using Game.Lib;
using UnityEngine;

namespace Game.Cards.Modifier
{
    [CreateAssetMenu(fileName = "Ignore Shields modifier", menuName = "Modifiers/Ignore Shields")]
    public sealed class ModirerIgnoreShields : ModifierBase
    {
        public override void Apply(ModifierContext context)
        {
            var defenceCards = context.DefendDeck;
            CardData shield = null; 

            if(defenceCards.GetCardCount() == 0) 
            {
                context.Player.Kill();
                return;
            }

            foreach (var card in defenceCards.cardDatas)
            {
                if(card == null) continue;

                if (card.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType != DefenceType.Shield)
                {
                    var sourceDamage = context.SourceCard.Parent.GetComponent<CardData>().GetCardFeature<AttackDecorator>().currentDamageValue;
                    card.GetCardFeature<HealthDecorator>().TakeDamage(sourceDamage);
                    return;
                }
                else shield = card;
            }

            if(shield != null) context.Player.Kill();
        }
    }
}