using System;
using UnityEngine;

namespace Game.Lib
{
    [Serializable]
    public class Health : IDamageble
    {
        private int value;
        private int maxValue;
        public int Value => value;
        public event Action OnDead;

        public Health(int healthValue)
        {
            this.maxValue = healthValue;
            this.value = healthValue;
        }

        public void TakeDamage(int amount)
        {
            if( amount <= 0 ) return;

            value -= amount;

            value = Mathf.Max( value, 0 );
            
            if( value <= 0 )
            {
                OnDead?.Invoke();
            }
        }

        public void Reset()
        {
            value = maxValue;
        }
    }

    public interface IDamageble
    {
        void TakeDamage(int amount);
    }
}