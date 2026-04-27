using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Cards
{
    public interface ICard : IDisposable
    {
        CardTypeEnum Type { get; }
        CardTypeEnum IgnoreLayers { get; }
        string CardName { get; }
        string Description { get; }
        bool isLocked { get; }
        Sprite Icon { get; }
        GameObject Parent { get; }
        CardData CardData { get; }

        void Start();
        void Use(GameObject target);
        void SetLock(bool enabled);
        void Destroy();
    }
}