using Game.Cards;
using Game.Lib;
using Unity.Collections;
using UnityEngine;

namespace Game.Deck
{
    public sealed class MonsterDeck : MonoBehaviour, IDeck<CardData>
    {
        [SerializeField] private int sizeDeck;
        [field:SerializeField, ReadOnly] public CardData[] cardDatas { get; private set; } // Карты монстров

        [SerializeField] private DefendDeck _defendDeck;
        private IDeck<CardData> defendDeck => _defendDeck;

        private void Awake()
        {
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
    }
}