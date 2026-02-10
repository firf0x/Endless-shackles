using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class AttackDecorator : CardDecorator
    {
        public readonly int defaultDamageValue;
        private int currentDamageValue;

        public AttackDecorator(int damageValue, ICard<CardTypeEnum> card) : base(card)
        {
            this.defaultDamageValue = damageValue;
        }

        public override void Use(GameObject target)
        {
            base.Use(target);

            CardData data = target.GetComponent<CardData>();


            if(data.TryGetCardFeature<IDamageble>(out var feature) && !data.decorateCard.IgnoreLayers.HasFlag(IgnoreLayers))
            {
                foreach (var effect in Effects)
                {
                    effect.Apply(this);
                }

                ToString();
                feature.ToString();
                feature.TakeDamage(currentDamageValue);
                feature.ToString();
            }

            //! Удаление карты атаки при нанесении урона по карте монстра
            if(data.decorateCard.Type.HasFlag(CardTypeEnum.Monster))
            {
                Parent.GetComponent<CardData>().CardDestroy();
            }
        }

        public void ChangeDamage(int value)
        {
            currentDamageValue = defaultDamageValue + value;
            currentDamageValue = Mathf.Abs(currentDamageValue);
        }

        public override string ToString()
        {
            string message = $"Attack: было нанесено {currentDamageValue} урона.";
            Debug.Log(message);
            return message;
        }
    }
}