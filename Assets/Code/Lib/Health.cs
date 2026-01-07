using System;
using UnityEngine;

namespace Lib
{
    [Serializable]
    public class Health : IDamageble
    {
        private int value { get; set; }
        public int Value => value;
        public bool isDie { get; private set; }
        public event Action OnDead;

        public Health(int healthValue)
        {
            this.value = healthValue;
        }        

        public void TakeDamage(int amount)
        {
            if( amount <= 0 ) return;

            value -= amount;

            amount = Mathf.Max( amount, 0 );
            
            if( value <= 0 )
            {
                isDie = true;
                OnDead?.Invoke();
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