using System.Collections.Generic;
using System.Linq;
using Game.Cards;
using Game.Lib;
using Unity.Collections;
using UnityEngine;

namespace Game.Deck
{
    public sealed class MonsterDeck : MonoBehaviour, IDeck<CardData>
    {
        public MonsterDeck Instance { get; private set; } // ? Я это когда писал?

        [SerializeField] private int sizeDeck;
        [field:SerializeField, ReadOnly] public CardData[] cardDatas { get; private set; } // Только карты монстров

        [Tooltip("Отступ между картами при распределении")]
        [SerializeField] private float Spacing = 0.1f;
        [SerializeField] private DeckBoard board;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }

            cardDatas = new CardData[sizeDeck];
        }

        private void OnValidate()
        {
            sizeDeck = Mathf.Max(sizeDeck, 0);
        }

        private void OnDestroy()
        {
            cardDatas = null;
        }

        public bool AddCard(CardData newCard)
        {
            // Ищем первую свободную ячейку
            int freeIndex = -1;
            for (int i = 0; i < cardDatas.Length; i++)
            {
                if (cardDatas[i] == null)
                {
                    freeIndex = i;
                    break;
                }
            }

            if (freeIndex >= 0)
            {
                // Добавляем карту в свободную ячейку
                cardDatas[freeIndex] = newCard;
                newCard.currentDeck = this;
                
                UpdateAllCardsPosition();
                return true;
            }
            else
            {
                Debug.LogWarning("Нет свободных слотов для карты!");
                return false;
            }
        }

        public void RemoveCard(CardData deletedCard, bool updateAllPosition)
        {
            for (int i = 0; i < cardDatas.Length; i++)
            {
                if(cardDatas[i] == deletedCard)
                {
                    cardDatas[i] = null;
                    break;
                }
            }

            if(updateAllPosition) UpdateAllCardsPosition();
        }

        /// <summary>
        /// Получить позицию для карты по индексу
        /// </summary>
        private Vector3 GetCardPosition(int index)
        {
            float startX = transform.position.x - board.Left + 1f;
            
            float posX = startX + (index * Spacing);
            float posY = ((board.Up - board.Down) / 2f) + transform.position.y; 
            float posZ = transform.position.z;
            
            return new Vector3(posX, posY, posZ);
        }

        /// <summary>
        /// Обновляет позиции всех карт в руке
        /// </summary>
        public void UpdateAllCardsPosition()
        {
            if (cardDatas == null) return;
            
            int insertPosition = 0;
            
            for (int i = 0; i < cardDatas.Length; i++)
            {
                if (cardDatas[i] != null)
                {
                    if (i != insertPosition)
                    {
                        // Перемещаем элемент на новую позицию
                        cardDatas[insertPosition] = cardDatas[i];
                        cardDatas[i] = null;
                    }
                    
                    // Устанавливаем позицию карты
                    Vector3 newPosition = GetCardPosition(insertPosition);
                    cardDatas[insertPosition].DeckPosition.Value = newPosition;
                    insertPosition++;
                }
            }
            
            for (int i = insertPosition; i < cardDatas.Length; i++)
            {
                cardDatas[i] = null;
            }
            
            // Debug.Log($"Сдвиг завершён. Активных карт: {insertPosition}");
        }

        private void OnDrawGizmos()
        {
            var leftUp = transform.position + new Vector3(-board.Left, board.Up);
            var leftDown = transform.position + new Vector3(-board.Left, -board.Down);
            var RightUp = transform.position + new Vector3(board.Right, board.Up);
            var RightDown = transform.position + new Vector3(board.Right, -board.Down);

            Gizmos.color = Color.red;

            Gizmos.DrawLine(leftUp, leftDown);
            Gizmos.DrawLine(leftDown, RightDown);
            Gizmos.DrawLine(RightDown, RightUp);
            Gizmos.DrawLine(RightUp, leftUp);


            Gizmos.color = Color.white;
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
                    if (cardDatas[i] != null) count++;
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
                    card.CardDestroy(false);
                }
            }
        }
    }
}