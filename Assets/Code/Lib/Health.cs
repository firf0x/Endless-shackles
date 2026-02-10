using System;
using UnityEngine;

namespace Game.Lib
{
    [Serializable]
    public class Health : IDamageble
    {
        private int value;
        public int Value => value;
        public bool isDie { get; private set; }
        public event Action OnDead;

        public Health(int healthValue)
        {
            this.value = healthValue;
        }

        public Health(int healthValue, Action subscribes)
        {
            this.value = healthValue;
            
            OnDead += subscribes;
        }

        public void TakeDamage(int amount)
        {
            if( amount <= 0 ) return;

            value -= amount;

            value = Mathf.Max( value, 0 );
            
            if( value <= 0 )
            {
                isDie = true;
                OnDead?.Invoke();
                
                OnDead = null;
            }
        }

        public void Restart()
        {
            isDie = false;
        }
    }

    public interface IDamageble
    {
        void TakeDamage(int amount);
    }
}