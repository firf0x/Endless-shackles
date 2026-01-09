using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Lib
{
    public interface ICard<T> where T : Enum
    {
        T Type { get; }
        T IgnoreLayers { get; }
        // poolCard
        string CardName { get; }
        string Description { get; }
        Sprite Icon { get; }
        List<EffectBase> Effects { get; }
        
        void Use(GameObject target);
        List<T> GetEffects<T>() where T : EffectBase;

        void OnReleaseToPool();
    }
}