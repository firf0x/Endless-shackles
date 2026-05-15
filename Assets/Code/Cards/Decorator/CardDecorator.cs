using System;
using System.Collections.Generic;
using Game.Cards.Modifier;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    [Serializable]
    public abstract class CardDecorator : ICard
    {
        protected ICard decoratedCard;

        public CardDecorator(ICard card)
        {
            decoratedCard = card;
        }

        public virtual CardTypeEnum Type => decoratedCard.Type;
        public virtual CardTypeEnum InteractionLayers => decoratedCard.InteractionLayers;
        public virtual string CardName => decoratedCard.CardName;
        public virtual string Description => decoratedCard.Description;
        public virtual bool isLocked => decoratedCard.isLocked;
        public bool isDrag => decoratedCard.isDrag;
        public virtual Sprite Icon => decoratedCard.Icon;
        public virtual GameObject Parent => decoratedCard.Parent;
        public CardData CardData => decoratedCard.CardData;

        public event Action OnDestroy
        {
            add => GetInnerCard().OnDestroy += value;
            remove => GetInnerCard().OnDestroy -= value;
        }

        public ICard GetInnerCard() => decoratedCard;
        public virtual void Start() => decoratedCard.Start();
        public virtual void Use(GameObject target) => decoratedCard.Use(target);
        public void SetLock(bool enabled) => decoratedCard.SetLock(enabled);
        public void SetDragActive(bool enabled) => decoratedCard.SetDragActive(enabled);
        public void Destroy() => decoratedCard.Destroy();

        public virtual void Dispose()
        {
            decoratedCard = null;
        }

    }
}