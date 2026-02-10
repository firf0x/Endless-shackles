using System;
using System.Collections.Generic;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    [Serializable]
    public abstract class CardDecorator : ICard<CardTypeEnum>
    {
        protected ICard<CardTypeEnum> decoratedCard;

        public CardDecorator(ICard<CardTypeEnum> card) => decoratedCard = card;

        public virtual CardTypeEnum Type => decoratedCard.Type;
        public virtual CardTypeEnum IgnoreLayers => decoratedCard.IgnoreLayers;
        public virtual string CardName => decoratedCard.CardName;
        public virtual string Description => decoratedCard.Description;
        public virtual Sprite Icon => decoratedCard.Icon;
        public virtual List<EffectBase<ICard<CardTypeEnum>>> Effects => decoratedCard.Effects;
        public virtual GameObject Parent 
        { 
            get => decoratedCard.Parent;
            set => decoratedCard.Parent = value;
        }

        public virtual List<T> GetEffects<T>() where T : EffectBase<ICard<CardTypeEnum>> => decoratedCard.GetEffects<T>();
        public virtual T GetEffect<T>() where T : EffectBase<ICard<CardTypeEnum>> => decoratedCard.GetEffect<T>();
        public ICard<CardTypeEnum> GetInnerCard() => decoratedCard;
        public virtual void Use(GameObject target) => decoratedCard.Use(target);

        public virtual void Dispose()
        {
            
            decoratedCard?.Dispose();
            decoratedCard = null;
        }
    }
}