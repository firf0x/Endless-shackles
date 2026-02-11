using Game.Cards;
using Game.Lib;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(fileName = "EffectIgnoreShields", menuName = "Game/CardEffect/EffectIgnoreShields")]
    public class EffectIgnoreShields : EffectBase<ICard<CardTypeEnum>>
    {
        public override bool Apply(ICard<CardTypeEnum> card)
        {
            // if(deck.player.GetComponent<CardData>().decorateCard is PlayerDecorator decorator)
            // {
            //     decorator.TakeDamage(100);
            //     return true;
            // }

            return false;
        }
    }
}
