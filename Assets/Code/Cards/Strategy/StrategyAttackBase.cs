using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    public abstract class StrategyAttackBase : IStrategy
    {
        public abstract string Name { get; }

        public abstract void Execute();

        public virtual void UpdateTarget(GameObject newTarget) { }
        public virtual void UpdateDamageValue(int damageValue) { }

        
        /// <summary>
        /// Проверяет, может ли стратегия атаковать данную цель.
        /// По умолчанию цель должна иметь компонент HealthDecorator.
        /// </summary>
        public virtual bool IsValidTarget(GameObject target)
        {
            if (target == null) return false;
            var cardData = target.GetComponent<CardData>();
            return cardData != null && cardData.TryGetCardFeature<HealthDecorator>(out _);
        }
    }
}