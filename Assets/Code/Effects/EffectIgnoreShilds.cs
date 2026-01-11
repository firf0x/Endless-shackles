using Game.Cards;
using Game.Deck;
using Game.Lib;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(fileName = "EffectIgnoreShilds", menuName = "Game/CardEffect/EffectIgnoreShilds")]
    public class EffectIgnoreShilds : EffectBase<ICard<CardTypeEnum>>
    {
        [SerializeField] private DefendDeck deck;
        public override bool Apply(ICard<CardTypeEnum> card)
        {
            foreach (var item in deck.cardDatas)
            {
                if(item.decorateCard is HealthDecorator decorator)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
