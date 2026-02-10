using Game.Cards;
using Game.Deck;
using Game.Lib;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(fileName = "EffectIgnoreShields", menuName = "Game/CardEffect/EffectIgnoreShields")]
    public class EffectIgnoreShields : EffectBase<ICard<CardTypeEnum>>
    {
        [SerializeField] private DefendDeck deck;
        public override bool Apply(ICard<CardTypeEnum> card)
        {
            foreach (var item in deck.cardDatas)
            {
                if(item.decorateCard is PlayerDecorator decorator)
                {
                    decorator.TakeDamage(100);
                    return true;
                }
            }
            return false;
        }
    }
}
