using UnityEngine;
using Game.Lib;

namespace Game.Cards
{
    public class HealthDecorator : CardDecorator, IDamageble
    {
        public Health healthSystem { get; private set; }
        
        /// <summary>
        /// Переменная отвечающая за то можно ли выбирать эту карту
        /// </summary>
        public bool isTarget = true;

        public HealthDecorator(int hp, ICard card) : base(card)
        {
            this.healthSystem = new Health(hp);
            this.healthSystem.OnDead += OnDead;
        }

        public void TakeDamage(int amount)
        {
            if(!isLocked)
            {
                healthSystem.TakeDamage(amount);
            }
        }

        public void Heal(int amount)
        {
            if(!isLocked)
            {
                healthSystem.Heal(amount);
            }
        }

        public void OnDead() => Destroy();

        public override void Dispose()
        {

            if (healthSystem != null)
            {
                healthSystem.OnDead -= OnDead;
            }
         
            base.Dispose();
        }
    }
}