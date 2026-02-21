using UnityEngine;
using System;
using Game.Cards;
using Game.Lib;
using Unity.Collections;
using Game.GameSystem;
using System.Collections.Generic;
using System.Linq;

namespace Game.Deck
{
    public class HandDeck : MonoBehaviour, IDeck<CardData>
    {
        public static HandDeck Instance { get; private set; }

        [SerializeField] private int sizeDeck;
        [SerializeField] private DeckBoard board;
        [SerializeField] private IDeck<CardData> defendDeck;
        
        [Tooltip("Отступ между картами при распределении")]
        [SerializeField] private float Spacing = 0.1f;

        [field:SerializeField, ReadOnly] public CardData[] cardDatas { get; private set; } // Только карты защиты
        private Transform[] cardPos;

        private void Start()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            
            Instance = this;

            cardPos = new Transform[sizeDeck];
            cardDatas = new CardData[sizeDeck];
        }

        private void OnValidate()
        {
            sizeDeck = Mathf.Max(sizeDeck, 0);
            Spacing = Mathf.Max(Spacing, 0);
            
            board.Left = Mathf.Max(board.Left, 0);
            board.Right = Mathf.Max(board.Right, 0);
            board.Up = Mathf.Max(board.Up, 0);
            board.Down = Mathf.Max(board.Down, 0);
        }

        /// <summary>
        /// Получить позицию для карты по индексу
        /// </summary>
        private Vector3 GetCardPosition(int index)
        {
            float startX = transform.position.x - board.Left;
            
            float posX = startX + (index * Spacing);
            float posY = transform.position.y;
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
                    cardDatas[insertPosition].transform.position = newPosition;
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

            Gizmos.color = Color.yellow;

            Gizmos.DrawLine(leftUp, leftDown);
            Gizmos.DrawLine(leftDown, RightDown);
            Gizmos.DrawLine(RightDown, RightUp);
            Gizmos.DrawLine(RightUp, leftUp);


            Gizmos.color = Color.white;
        }

        /// <summary>
        /// Добавляет новую карту в руку
        /// </summary>
        public void AddCard(CardData newCard)
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

                UpdateAllCardsPosition();
            }
            else
            {
                Debug.LogWarning("Нет свободных слотов для карты!");
            }
        }

        /// <summary>
        /// Удаляет карту по индексу
        /// </summary>
        public void RemoveCard(CardData deletedCard, bool isClearAll)
        {
            for (int i = 0; i < cardDatas.Length; i++)
            {
                if(cardDatas[i] == deletedCard)
                {
                    cardDatas[i] = null;
                    break;
                }
            }

            if(!isClearAll) UpdateAllCardsPosition();
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
                if(card != null)
                {
                    card.CardDestroy(true);
                }
            }
        }
    }

    [Serializable]
    public struct DeckBoard
    {
        public float Up;
        public float Down;
        public float Left;
        public float Right;
    }
}