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
    }
}