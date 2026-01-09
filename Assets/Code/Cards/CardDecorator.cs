using System;
using System.Collections.Generic;
using Lib;
using UnityEngine;

namespace Game.Cards
{
    [Serializable]
    public abstract class CardDecorator : ICard
    {
        protected ICard decoratedCard;

        public CardDecorator(ICard card) => decoratedCard = card;

        public virtual CardTypeEnum Type => decoratedCard.Type;
        public virtual string CardName => decoratedCard.CardName;
        public virtual string Description => decoratedCard.Description;
        public virtual Sprite Icon => decoratedCard.Icon;
        public virtual List<EffectBase> Effects => decoratedCard.Effects;

        public virtual List<T> GetEffects<T>() where T : EffectBase => decoratedCard.GetEffects<T>();

        public virtual void OnReleaseToPool() => decoratedCard.OnReleaseToPool();

        public virtual void Use(GameObject target) => decoratedCard.Use(target);
    }
}