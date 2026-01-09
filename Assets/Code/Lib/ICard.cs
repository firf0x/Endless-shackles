using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Lib
{
    public interface ICard<TEnum> where TEnum : Enum
    {
        TEnum Type { get; }
        TEnum IgnoreLayers { get; }
        IPool<ICard<TEnum>> Pool { get; set; }
        string CardName { get; }
        string Description { get; }
        Sprite Icon { get; }
        List<EffectBase> Effects { get; }
        GameObject Parent { get; set; }

        void Use(GameObject target);
        List<T> GetEffects<T>() where T : EffectBase;

        void OnReleaseToPool();
    }
}