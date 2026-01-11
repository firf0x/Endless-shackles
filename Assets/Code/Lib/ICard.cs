using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Lib
{
    public interface ICard<TEnum> where TEnum : Enum
    {
        TEnum Type { get; }
        TEnum IgnoreLayers { get; }
        string CardName { get; }
        string Description { get; }
        Sprite Icon { get; }
        List<EffectBase<ICard<TEnum>>> Effects { get; }
        GameObject Parent { get; set; }

        void Use(GameObject target);
        List<T> GetEffects<T>() where T : EffectBase<ICard<TEnum>>;
        T GetEffect<T>() where T : EffectBase<ICard<TEnum>>;
    }
}