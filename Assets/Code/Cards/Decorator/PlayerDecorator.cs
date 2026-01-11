using UnityEngine;
using Game.Lib;

namespace Game.Cards
{
    public class PlayerDecorator : CardDecorator, IDamageble
    {
        private Health healthSystem;

        public PlayerDecorator(ICard<CardTypeEnum> card) : base(card)
        {
            this.healthSystem = new Health(1);
        }

        public void TakeDamage(int amount)
        {
            healthSystem.TakeDamage(amount);
        }

        public override string ToString()
        {
            string message = $"Health: текущее количество {healthSystem.Value} здоровья.";
            Debug.Log(message);
            return message;
        }
    }
}