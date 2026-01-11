using Game.Cards;
using Game.Lib;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(fileName = "EffectRandomDamage", menuName = "Game/CardEffect/EffectRandomDamage", order = 1)]
    public class EffectRandomDamage : EffectBase<ICard<CardTypeEnum>>
    {
        [SerializeField] private int StartDamage;
        [SerializeField] private int EndDamage;

        public override bool Apply(ICard<CardTypeEnum> card)
        {
            if(card is AttackDecorator decorator)
            {
                decorator.ChangeDamage(-decorator.defaultDamageValue);
                decorator.ChangeDamage(Random.Range(StartDamage, EndDamage + 1));
            }

            return true;
        }
    }
}