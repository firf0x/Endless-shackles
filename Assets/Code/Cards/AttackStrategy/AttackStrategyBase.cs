using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    [Serializable]
    public abstract class AttackStrategyBase : IAttackStrategy
    {
        public abstract string Name { get; }
        public virtual void Init() { }
        public abstract void Execute(CombatArgs args);
        public virtual void OnRemove() { }
    }
}