using System;
using UnityEngine;

namespace Lib
{
    [Serializable]
    public class Health : IDamageble
    {
        [SerializeField] private int defaultValue;
        private int value;
        public int Value => value;
        public bool isDie { get; private set; }
        public event Action OnDead;
        

        public void TakeDamage(int amount)
        {
            if( amount <= 0 ) return;

            amount = Mathf.Max( 0, amount );

            value -= amount;
            
            if( value <= 0 )
            {
                isDie = true;
                OnDead?.Invoke();
            }
        }
    
        public void Restart()
        {
            value = defaultValue;
        }
    }

    public interface IDamageble
    {
        void TakeDamage(int amount);
    }
}