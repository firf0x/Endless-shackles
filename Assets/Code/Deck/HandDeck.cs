using UnityEngine;
using System;
using Game.Cards;
using Game.Deck;

public class HandDeck : MonoBehaviour, IDeck
{
    [SerializeField] private int sizeDeck;
    [SerializeField] private DeckBoard board;

    public CardData[] cardDatas { get; private set; }
    private Transform[] cardPoint;

    private void Awake()
    {
        cardDatas = new CardData[sizeDeck];
        cardPoint = new Transform[sizeDeck];
    }

    private void OnValidate()
    {
        sizeDeck = Mathf.Max(sizeDeck, 0);
        
        board.Left = Mathf.Max(board.Left, 0);
        board.Right = Mathf.Max(board.Right, 0);
        board.Up = Mathf.Max(board.Up, 0);
        board.Down = Mathf.Max(board.Down, 0);
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
    /// Распределяет карты в пределах области board
    /// </summary>
    private void ArrangeCards()
    {
        for (int i = 0; i < cardDatas.Length; i++)
        {
            Vector3 cardPosition = CalculateCardPosition(i);
            
            if (cardPoint[i] != null)
            {
                cardPoint[i].transform.localPosition = cardPosition - transform.position;
            }
        }
    }

    /// <summary>
    /// Вычисляет позицию для карты с индексом i
    /// </summary>
    private Vector3 CalculateCardPosition(int index)
    {
        if (cardDatas.Length <= 1)
        {
            // Если карта одна - размещаем по центру
            return transform.position;
        }

        // Распределяем карты по горизонтали
        float totalWidth = board.Left + board.Right;
        float startX = transform.position.x - board.Left;
        float endX = transform.position.x + board.Right;
        
        // Линейное распределение
        float t = (float)index / (cardDatas.Length - 1);
        float xPos = Mathf.Lerp(startX, endX, t);
        
        
        float yPos = transform.position.y;
        
        return new Vector3(xPos, yPos, 0);
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

        // Добавляем карту в свободную ячейку
        cardDatas[freeIndex] = newCard;

        /*
            Type:
            A = Attack
            D = Defence
        */
        for (int A = 0; A < sizeDeck - 1; A++)
        {
            if (cardDatas[A] == null) continue;
            
            for (int D = A + 1; D < sizeDeck; D++)
            {
                if (cardDatas[D] == null) continue;
                
                // Получаем приоритеты типов карт
                int priorityA = (int)cardDatas[A].cardBase.Type;
                int priorityD = (int)cardDatas[D].cardBase.Type;
                
                // Если приоритет текущей карты больше, чем следующей - меняем местами
                if (priorityA > priorityD)
                {
                    CardData temp = cardDatas[A];
                    cardDatas[A] = cardDatas[D];
                    cardDatas[D] = temp;
                }
            }
        }

        // Перераспределяем все карты
        ArrangeCards();
    }

    /// <summary>
    /// Удаляет карту по индексу
    /// </summary>
    public void RemoveCard(CardData deletedCard)
    {

        for (int i = -1; i < cardDatas.Length; i++)
        {
            if(cardDatas[i] == deletedCard)
            {
                cardDatas[i] = null;
                break;
            }
        }

        // Перераспределяем оставшиеся карты
        ArrangeCards();
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