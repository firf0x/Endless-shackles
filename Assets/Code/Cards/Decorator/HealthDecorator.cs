using UnityEngine;
using Game.Lib;

namespace Game.Cards
{
    public class HealthDecorator : CardDecorator, IDamageble
    {
        public Health healthSystem { get; private set; }

        public HealthDecorator(int hp, ICard<CardTypeEnum> card) : base(card)
        {
            this.healthSystem = new Health(hp);
            this.healthSystem.OnDead += OnDead;
        }

        public void TakeDamage(int amount)
        {
            healthSystem.TakeDamage(amount);
        }

        public void OnDead()
        {
            Parent.GetComponent<CardData>().CardDestroy();
        }

        public override string ToString()
        {
            string message = $"Health: текущее количество {healthSystem.HealPoints.Value} здоровья.";
            Debug.Log(message);
            return message;
        }

        public override void Dispose()
        {
            if (healthSystem != null)
            {
                healthSystem.OnDead -= OnDead;
                healthSystem = null;
            }
        }
    }
}