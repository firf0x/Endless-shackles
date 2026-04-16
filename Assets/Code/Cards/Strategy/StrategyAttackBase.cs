using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    [Serializable]
    public abstract class StrategyAttackBase : IAttackStrategy
    {
        public abstract string Name { get; }
        public virtual void Init() { }
        public abstract void Execute(GameObject parent, GameObject target, int damage);
    }
}