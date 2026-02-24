using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Lib
{
    public interface ICard<TEnum> : IDisposable where TEnum : Enum
    {
        TEnum Type { get; }
        TEnum IgnoreLayers { get; }
        string CardName { get; }
        string Description { get; }
        Sprite Icon { get; }
        GameObject Parent { get; set; }

        void Use(GameObject target);
    }
}