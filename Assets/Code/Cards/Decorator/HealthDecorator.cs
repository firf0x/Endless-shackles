using UnityEngine;
using Game.Lib;

namespace Game.Cards
{
    public class HealthDecorator : CardDecorator, IDamageble
    {
        private Health healthSystem;

        public HealthDecorator(int hp, ICard<CardTypeEnum> card) : base(card)
        {
            this.healthSystem = new Health(hp);
        }

        public void TakeDamage(int amount)
        {
            healthSystem.TakeDamage(amount);
        }

        public override void OnReleaseToPool()
        {
            base.OnReleaseToPool();
            
            healthSystem.Restart();
        }

        public override string ToString()
        {
            string message = $"Health: текущее количество {healthSystem.Value} здоровья.";
            Debug.Log(message);
            return message;
        }
    }
}