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
        public CardData[] cardDatas 
        {
            get
            {
                return cards.Values.ToArray();
            }
        }

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

        public bool AddCard(CardData newCard)
        {
            if(newCard == null) return false;

            var key = newCard.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType;
            
            if (cards.ContainsKey(key))
            {
                // Перезаписываем
                if (cards[key] == null)
                {
                    cards[key] = newCard;
                    newCard.currentDeck = this;
                    
                    Debug.Log($"Ключ {key} существовал с null, перезаписан");
                    return true;
                }
                else 
                {
                    Debug.Log($"Ошибка добавления {newCard} - ключ {key} уже существует");
                    return false;
                }
            }
            else
            {
                cards.Add(key, newCard);
                newCard.currentDeck = this;
                Debug.Log($"Добавлена новая карта");
                return true;
            }
        }

        public void RemoveCard(CardData deletedCard, bool isClearAll)
        {
            if (isClearAll) cards.Clear();
            else
            {
                // Ищем ключ по значению
                DefenceType? keyToRemove = null;
                foreach (var card in cards)
                {
                    if (card.Value == deletedCard)
                    {
                        keyToRemove = card.Key;
                        break;
                    }
                }
                
                if (keyToRemove.HasValue) cards.Remove(keyToRemove.Value);
            }
        }

        /// <summary>
        /// Получить общее количество карт в руке
        /// </summary>
        public int GetCardCount()
        {
            return cards.Values.Count(card => card != null);
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

        public bool ContainsCard(DefenceType defenceType)
        {
            return cards != null && cards.ContainsKey(defenceType);
        }

        //! Update data!!! обновляется список возможных карт которым можно нанести урон.
        public void UpdateAllCardsPosition()
        {
        }
    }
}