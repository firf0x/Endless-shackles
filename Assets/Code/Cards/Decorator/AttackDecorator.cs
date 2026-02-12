using System.Collections.Generic;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class AttackDecorator : CardDecorator
    {
        public readonly int defaultDamageValue;
        public int currentDamageValue { get; private set; }

        public AttackDecorator(int damageValue, ICard<CardTypeEnum> card) : base(card)
        {
            this.defaultDamageValue = damageValue;
            ChangeDamage(0);
        }

        public override void Use(GameObject target)
        {
            base.Use(target);

            CardData data = target.GetComponent<CardData>();

            if(data.TryGetCardFeature<HealthDecorator>(out var feature) && !data.decorateCard.IgnoreLayers.HasFlag(IgnoreLayers))
            {
                foreach (var effect in Effects)
                {
                    effect.Apply(this);
                }

                //! Удаление карты атаки при нанесении урона по карте монстра
                if(data.decorateCard.Type.HasFlag(CardTypeEnum.Monster))
                {
                    Parent.GetComponent<CardData>().CardDestroy(false);
                }

                feature.TakeDamage(currentDamageValue);
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