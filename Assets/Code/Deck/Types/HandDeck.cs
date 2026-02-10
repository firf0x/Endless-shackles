using UnityEngine;
using System;
using Game.Cards;
using Game.Lib;
using Unity.Collections;

public class HandDeck : MonoBehaviour, IDeck<CardData>
{
    [SerializeField] private int sizeDeck;
    [SerializeField] private DeckBoard board;
    [SerializeField] private IDeck<CardData> defendDeck;
    
    [Tooltip("Если true, карты распределяются горизонтально. Если false - вертикально")]
    [SerializeField] private bool horizontalDistribution = true;
    
    [Tooltip("Отступ между картами при распределении")]
    [SerializeField] private float cardSpacing = 0.1f;

    [field:SerializeField, ReadOnly] public CardData[] cardDatas { get; private set; } // Карты этой колоды
    private Transform[] cardPoint;

    private void Awake()
    {
        cardDatas = new CardData[sizeDeck];
        cardPoint = new Transform[sizeDeck];
    }

    private void OnValidate()
    {
        sizeDeck = Mathf.Max(sizeDeck, 0);
        cardSpacing = Mathf.Max(cardSpacing, 0);
        
        board.Left = Mathf.Max(board.Left, 0);
        board.Right = Mathf.Max(board.Right, 0);
        board.Up = Mathf.Max(board.Up, 0);
        board.Down = Mathf.Max(board.Down, 0);
    }

    /// <summary>
    /// Получить позицию для карты по индексу
    /// </summary>
    public Vector3 GetCardPosition(int index)
    {
        int filledSlots = 0;

        for (int i = 0; i < cardDatas.Length; i++)
        {
            if (cardDatas[i] != null)
                filledSlots++;
        }
        
        if (filledSlots <= 0)
            return transform.position;
        
        Vector3 basePosition = transform.position;
        
        float totalWidth = board.Left + board.Right;
        float spacing = 1f; // Отступ между картами
        
        if (filledSlots > 1)
        {
            spacing = totalWidth / (filledSlots - 1);
        }
        
        // Начинаем с левого края
        float startX = basePosition.x - board.Left;
        float xPos = startX + (index * spacing);
        
        // Вертикальная позиция (центр по вертикали)
        float yPos = basePosition.y + ((board.Up - board.Down) * 0.5f);
        
        return new Vector3(xPos, yPos, basePosition.z);
    }

    /// <summary>
    /// Обновить позиции всех карт
    /// </summary>
    public void UpdateCardPositions()
    {
        if (cardDatas == null || cardPoint == null)
            return;
            
        int currentIndex = 0;
        for (int i = 0; i < cardDatas.Length; i++)
        {
            if (cardDatas[i] != null && cardPoint[i] != null)
            {
                cardPoint[i].position = GetCardPosition(currentIndex);
                currentIndex++;
            }
        }
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

        // Рисуем точки для распределения карт
        if (cardDatas != null && cardDatas.Length > 0)
        {
            Gizmos.color = Color.green;
            int filledSlots = 0;
            for (int i = 0; i < cardDatas.Length; i++)
            {
                if (cardDatas[i] != null)
                    filledSlots++;
            }
            
            for (int i = 0; i < filledSlots; i++)
            {
                Vector3 pos = GetCardPosition(i);
                Gizmos.DrawSphere(pos, 0.1f);
            }
        }

        Gizmos.color = Color.white;
    }

    /// <summary>
    /// Добавляет новую карту в руку
    /// </summary>
    public void AddCard(CardData newCard)
    {   
        // Ищем первую свободную ячейку
        int freeIndex = -1;
        for (int i = 0; i < sizeDeck; i++)
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
            // Обновляем позиции всех карт
            UpdateCardPositions();
        }
        else
        {
            Debug.LogWarning("Нет свободных слотов для карты!");
        }
    }

    /// <summary>
    /// Удаляет карту по индексу
    /// </summary>
    public void RemoveCard(CardData deletedCard)
    {
        for (int i = 0; i < cardDatas.Length; i++)
        {
            if(cardDatas[i] == deletedCard)
            {
                cardDatas[i] = null;
                // Обновляем позиции всех карт
                UpdateCardPositions();
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
            for (int i = 0; i < cardDatas.Length; i++)
            {
                if (cardDatas[i] != null)
                    count++;
            }
        }
        return count;
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