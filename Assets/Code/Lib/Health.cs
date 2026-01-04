using System;
using UnityEngine;

namespace Lib
{
    [Serializable]
    public class Health : IDamageble
    {
        public int Value => value;
        public bool isDie { get; private set; }
        public event Action OnDead;

        [SerializeField] private int value;

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
    }

    public interface IDamageble
    {
        void TakeDamage(int amount);
    }
}