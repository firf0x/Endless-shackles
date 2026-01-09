using System;
using System.Collections.Generic;
using Game.Cards;
using Game.Deck;
using Game.Deck.Fabric;
using Game.Lib;
using UnityEngine;

namespace Game.Deck.Pool
{
    public class PoolCard : IPool<ICard<CardTypeEnum>>
    {
        private Queue<ICard<CardTypeEnum>> availableObjects;
        private List<ICard<CardTypeEnum>> occupiedObjects;
        private Creator<ICard<CardTypeEnum>> fabric;

        public PoolCard(DeckCardsConfig config)
        {
            this.availableObjects = new Queue<ICard<CardTypeEnum>>();
            this.occupiedObjects = new List<ICard<CardTypeEnum>>();
            this.fabric = new CardCreator(config);
        }

        public ICard<CardTypeEnum> Get(Transform transform)
        {
            ICard<CardTypeEnum> card = null;
            
            if (availableObjects.Count > 0) card = availableObjects.Dequeue();
            else card = fabric.Create();

            card.Pool = this;

            occupiedObjects.Add(card);

            return card;
        }

        public void Release(ICard<CardTypeEnum> card)
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