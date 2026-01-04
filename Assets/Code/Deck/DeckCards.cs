using System.Collections.Generic;
using Game.Cards;
using UnityEngine;

namespace Object.Deck
{
    [CreateAssetMenu(fileName = "DeckCard", menuName = "Game/DeckCard", order = 0)]
    public class DeckCards : ScriptableObject
    {
        [SerializeField] private List<CardAttack> cardsAttack = new List<CardAttack>();
        [SerializeField] private List<CardDefence> cardsDefence = new List<CardDefence>();
        [SerializeField] private List<CardMonster> cardsMonster = new List<CardMonster>();

        [SerializeField] public GameObject prefabCardAttack;
        [SerializeField] public GameObject prefabCardDefence;
        [SerializeField] public GameObject prefabCardMonster;

        public int CountCards => cardsAttack.Count + cardsDefence.Count + cardsMonster.Count;

        public int CountCardsAttack => cardsAttack.Count;
        public int CountCardsDefence => cardsDefence.Count;
        public int CountCardsMonster => cardsDefence.Count;

        public CardBase GetCard( CardTypeEnum type, int index )
        {
            switch (type)
            {
                case CardTypeEnum.Attack: return cardsAttack[index];
                case CardTypeEnum.Defence: return cardsDefence[index];
                case CardTypeEnum.Monster: return cardsMonster[index];
                default:
                    Debug.LogError("Такого типа карты не существует.");
                    return null;
            }
        }

        public CardBase GetRandomCard( CardTypeEnum type )
        {
            switch (type)
            {
                case CardTypeEnum.Attack: return cardsAttack[Random.Range(0, cardsAttack.Count)];
                case CardTypeEnum.Defence: return cardsDefence[Random.Range(0, cardsDefence.Count)];
                case CardTypeEnum.Monster: return cardsMonster[Random.Range(0, cardsMonster.Count)];
                default:
                    Debug.LogError("Такого типа карты не существует.");
                    return null;
            }
        }
    }
}