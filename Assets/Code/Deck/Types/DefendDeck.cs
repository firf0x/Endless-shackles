using System.Collections.Generic;
using System.Linq;
using Game.Cards;
using Game.Lib;
using Unity.Collections;
using UnityEngine;

namespace Game.Deck
{
    public sealed class DefendDeck : MonoBehaviour, IDeck<CardData>
    {
        public static DefendDeck Instance { get; private set; }
        [SerializeField] private int sizeDeck;
        [field:SerializeField, ReadOnly] public Dictionary<DefenceType, CardData> cards { get; private set; } // Только карты защиты
        public IReadOnlyList<CardData> cardDatas => cards.Values.ToList();
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            cards = new Dictionary<DefenceType, CardData>();
        }

        private void OnValidate()
        {
            sizeDeck = Mathf.Max(sizeDeck, 0);
        }

        public void AddCard(CardData newCard)
        {
            cards.Add(newCard.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType, newCard);
        }

        public void RemoveCard(CardData deletedCard, bool isClearAll)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                if(cardDatas[i] == deletedCard)
                {
                    cards.Remove(deletedCard.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType);
                    break;
                }
            }
        }

        /// <summary>
        /// Получить общее количество карт в руке
        /// </summary>
        public int GetCardCount()
        {
            int count = 0;
            if (cardDatas != null)
            {
                for (int i = 0; i < cardDatas.Count; i++)
                {
                    if (cardDatas[i] != null)
                        count++;
                }
            }
            return count;
        }

        public void ClearCards()
        {
            if(GetCardCount() <= 0) return;

            foreach (var card in cardDatas)
            {
                if(card != null)
                {
                    card.CardDestroy(true);
                }
            }
        }

        public void UpdateAllCardsPosition() { }
    }
}