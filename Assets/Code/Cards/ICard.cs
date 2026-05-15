using System.Collections.Generic;
using UnityEngine;
using System;

namespace Game.Cards
{
    public interface ICard : IDisposable
    {
        CardTypeEnum Type { get; }
        CardTypeEnum InteractionLayers { get; }
        string CardName { get; }
        string Description { get; }
        bool isLocked { get; }
        bool isDrag { get; }
        Sprite Icon { get; }
        GameObject Parent { get; }
        CardData CardData { get; }
        event Action OnDestroy;

        void Start();
        void Use(GameObject target);
        void SetLock(bool enabled);
        void SetDragActive(bool enabled);
        void Destroy();
    }
}