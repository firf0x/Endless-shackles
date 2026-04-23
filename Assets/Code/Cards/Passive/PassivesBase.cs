using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Passive
{
    [Serializable]
    public abstract class PassivesBase : IPassiveStrategy
    {
        public virtual void Init(GameObject parent) { }
        public virtual void Apply(GameObject parent) { }
        public virtual void OnUpdate(GameObject parent) { }
        public virtual void OnRemove() { }
    }
}