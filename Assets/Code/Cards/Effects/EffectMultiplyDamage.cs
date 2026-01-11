using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    [CreateAssetMenu(fileName = "EffectMultiplyDamage", menuName = "Game/CardEffect/EffectMultiplyDamage", order = 0)]
    public class EffectMultiplyDamage : EffectBase<ICard<CardTypeEnum>>
    {
        [SerializeField] private int Damage;
        public override bool Apply(ICard<CardTypeEnum> card)
        {
            if(card is AttackDecorator decorator)
            {
                decorator.ChangeDamage(decorator.defaultDamageValue);
            }

            return true;
        }

        public override string ToString()
        {
            string message = $"Attack: было нанесено {Damage} урона.";
            Debug.Log(message);
            return message;
        }
    }
}