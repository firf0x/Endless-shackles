using System.Collections.Generic;
using Game.Cards;
using Game.Lib;
using UnityEngine;

namespace Game.Effects
{
    [CreateAssetMenu(fileName = "EffectMultiplyDamage", menuName = "Game/CardEffect/EffectMultiplyDamage", order = 0)]
    public class EffectMultiplyDamage : EffectBase<ICard<CardTypeEnum>>
    {
        [SerializeField] private int DamageMultiply;
        public override bool Apply(ICard<CardTypeEnum> card)
        {
            if(card is AttackDecorator decorator)
            {
                decorator.ChangeDamage(decorator.defaultDamageValue * (DamageMultiply - 1));
            }

            return true;
        }

        public override string ToString()
        {
            string message = $"Attack: было умножено {DamageMultiply} урона.";
            Debug.Log(message);
            return message;
        }
    }
}