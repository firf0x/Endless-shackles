using System;
using UnityEngine;

namespace Game.Lib
{
    [Serializable]
    public class Health : IDamageble
    {
        //! ВЫЗЫВАТЬ МЕТОДЫ НА ПРЯМУЮ СТРОГО ЗАПРЕЩАЕТСЯ!

        // PUBLIC
        public ReactiveProperty<int> HealPoints { get; private set; } = new();
        public event Action OnDead;
     
        // PRIVATE
        private int maxValue;

        public Health(int healthValue)
        {
            this.maxValue = healthValue;
            
            HealPoints.Value = healthValue;
        }

        public void TakeDamage(int amount)
        {
            if( amount <= 0 ) return;

            int current = HealPoints.Value;

            current -= amount;

            HealPoints.Value = Mathf.Max( current, 0 );
            
            if( HealPoints.Value <= 0 )
            {
                OnDead?.Invoke();
            }
        }

        public void Heal(int amount)
        {
            int current = HealPoints.Value; 
            current += amount;
        
            HealPoints.Value = Mathf.Min( current, maxValue );
        }

        public void Reset()
        {
            HealPoints.Value = maxValue;
        }
    }

    public interface IDamageble
    {
        void TakeDamage(int amount);
        void Heal(int amount);
    }
}