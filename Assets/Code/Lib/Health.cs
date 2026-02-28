using System;
using UnityEngine;

namespace Game.Lib
{
    [Serializable]
    public class Health : IDamageble
    {
        // PUBLIC
        public ReactiveProperty<int> HealPoints { get; private set; } = new();
        public event Action OnHealPointsChanged;
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

            HealPoints.Value -= amount;

            HealPoints.Value = Mathf.Max( HealPoints.Value, 0 );
            
            if( HealPoints.Value <= 0 )
            {
                OnDead?.Invoke();
            }
            else OnHealPointsChanged?.Invoke();
        }

        public void Heal(int amount)
        {
            HealPoints.Value += amount;
        
            HealPoints.Value = Mathf.Min( HealPoints.Value, maxValue );
            
            OnHealPointsChanged?.Invoke();
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