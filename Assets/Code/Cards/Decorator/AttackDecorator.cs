using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class AttackDecorator : CardDecorator
    {
        private int defaultDamageValue;
        private int currentDamageValue;

        public AttackDecorator(int damageValue, ICard<CardTypeEnum> card) : base(card)
        {
            this.defaultDamageValue = damageValue;
        }

        public override void Use(GameObject target)
        {
            base.Use(target);
            
            CardData data = target.GetComponent<CardData>();

            currentDamageValue = defaultDamageValue;


            if(data.TryGetCardFeature<IDamageble>(out var feature) && !data.decorateCard.IgnoreLayers.HasFlag(IgnoreLayers))
            {
                ToString();
                feature.ToString();
                feature.TakeDamage(currentDamageValue);
                feature.ToString();
            }

            if(Type == CardTypeEnum.Attack) ;
        }

        public override string ToString()
        {
            string message = $"Attack: было нанесено {currentDamageValue} урона.";
            Debug.Log(message);
            return message;
        }
    }
}