using System;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Passive
{
    [Serializable]
    public abstract class PassivesBase : IPassiveStrategy
    {
        public virtual void Init(PassiveContext context) { }
        public virtual void OnGeneralUpdate(PassiveContext context) { }
        public virtual void OnUpdate(PassiveContext context) { }
        public virtual void OnRemove(PassiveContext context) { }
        public virtual void OnCallBack(PassiveContext context) { }
    }
}