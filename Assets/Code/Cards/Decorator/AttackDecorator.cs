using UnityEngine;
using Lib;

namespace Game.Cards
{
    public class AttackDecorator : CardDecorator
    {
        private int defaultDamageValue;
        private int currentDamageValue;

        public AttackDecorator(int damageValue, ICard card) : base(card)
        {
            this.defaultDamageValue = damageValue;
        }

        public override void Use(GameObject target)
        {
            base.Use(target);
            
            currentDamageValue = defaultDamageValue;

            if(target.GetComponent<CardData>().TryGetCardFeature<IDamageble>(out var feature))
            {
                ToString();
                feature.ToString();
                feature.TakeDamage(currentDamageValue);
                feature.ToString();
            }
        }

        public override string ToString()
        {
            string message = $"Attack: было нанесено {currentDamageValue} урона.";
            Debug.Log(message);
            return message;
        }
    }
}