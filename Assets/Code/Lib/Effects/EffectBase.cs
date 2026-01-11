using System;
using UnityEngine;

namespace Game.Lib
{    
    public abstract class EffectBase<T> : ScriptableObject, IEffectHandler<T>
    {
        [SerializeField] private string effectName;
        [SerializeField] private string effectDiscription;

        public string Name => effectName;
        public string Discription => effectDiscription;

        public abstract bool Apply(T card);
    }
}