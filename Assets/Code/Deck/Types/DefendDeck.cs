using Game.Cards;
using Game.Lib;
using Unity.Collections;
using UnityEngine;

namespace Game.Deck
{
    public sealed class DefendDeck : MonoBehaviour, IDeck<CardData>
    {
        public DefendDeck Instance { get; private set; }
        [SerializeField] private int sizeDeck;
        [field:SerializeField, ReadOnly] public CardData[] cardDatas { get; private set; } // Только карты защиты
        [field:SerializeField] public GameObject player { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;
            cardDatas = new CardData[sizeDeck];
        }

        private void OnValidate()
        {
            sizeDeck = Mathf.Max(sizeDeck, 0);
        }

        public void AddCard(CardData newCard)
        {
            int freeIndex = -1;
            for (int i = 0; i < sizeDeck; i++)
            {
                if (cardDatas[i] == null)
                {
                    freeIndex = i;
                    break;
                }
            }

            cardDatas[freeIndex] = newCard;
        }

        public void RemoveCard(CardData deletedCard)
        {
            
        }

        /// <summary>
        /// Получить общее количество карт в руке
        /// </summary>
        public int GetCardCount()
        {
            int count = 0;
            if (cardDatas != null)
            {
                for (int i = 0; i < cardDatas.Length; i++)
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
                if(card != null) card.CardDestroy();
                player.GetComponent<CardData>();
            }
        }
    }
}