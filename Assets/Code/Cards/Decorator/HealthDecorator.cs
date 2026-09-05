using UnityEngine;
using Game.Lib;

namespace Game.Cards
{
    public class HealthDecorator : CardDecorator, IDamageble
    {
        public Health healthSystem { get; private set; }
        
        /// <summary>
        /// Переменная отвечающая за то можно ли выбирать эту карту противниками
        /// </summary>
        public bool isTarget = true;

        public HealthDecorator(int hp, ICard card) : base(card)
        {
            this.healthSystem = new Health(hp);
            healthSystem.OnDead += OnDead;
        }

        public void TakeDamage(int amount)
        {
            if(!isLocked)
            {
                healthSystem.TakeDamage(amount);
            }
        }

        public void OnDead() => Destroy();

        public void Heal(int amount)
        {
            if(!isLocked)
            {
                healthSystem.Heal(amount);
            }
        }

        public override void Dispose()
        {
            healthSystem.OnDead -= OnDead;
            base.Dispose();
        }
    }
}