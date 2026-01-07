using System;
using UnityEngine;

namespace Lib
{
    [Serializable]
    public class Health : IDamageble, IEffectHandler
    {
        [SerializeField] private int defaultValue;
        private int value { get; set; }
        public int Value => value;
        public bool isDie { get; private set; }
        public event Action OnDead;
        

        public void TakeDamage(int amount)
        {
            if( amount <= 0 ) return;

            value -= amount;

            amount = Mathf.Max( 0, amount );
            
            if( value <= 0 )
            {
                isDie = true;
                OnDead?.Invoke();
            }
        }

        public bool Apply(GameObject target)
        {
            return true;
        }
    
        public void Restart()
        {
            value = defaultValue;
            isDie = false;
        }
    }

    public interface IDamageble
    {
        void TakeDamage(int amount);
    }
}