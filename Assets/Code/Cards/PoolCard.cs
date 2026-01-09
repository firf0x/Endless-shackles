using System;
using System.Collections.Generic;
using Lib;
using UnityEngine;

namespace Game.Cards
{
    public class PoolCard : IDisposable
    {
        private Queue<ICard> availableObjects;
        private List<ICard> occupiedObjects;
        private readonly Func<ICard> createNewCard;

        public PoolCard(Func<ICard> createNewCard)
        {
            this.availableObjects = new Queue<ICard>();
            this.occupiedObjects = new List<ICard>();
            this.createNewCard = createNewCard;
        }

        public ICard Get(Transform transform)
        {
            ICard card = null;
            
            if (availableObjects.Count > 0) card = availableObjects.Dequeue();
            else card = createNewCard?.Invoke();

            occupiedObjects.Add(card);

            return card;
        }

        public void Release(ICard card)
        {
            if (card == null) return;
            
            if(occupiedObjects.Contains(card))
            {
                occupiedObjects.Remove(card);

                card.OnReleaseToPool();
            }
            
            availableObjects.Enqueue(card);
        }

        public void Dispose()
        {
            availableObjects.Clear();
            occupiedObjects.Clear();
        }
    }
}