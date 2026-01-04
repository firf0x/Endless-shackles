using System;
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

        [SerializeField] private GameObject prefabCardAttack;
        [SerializeField] private GameObject prefabCardDefence;
        [SerializeField] private GameObject prefabCardMonster;



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
    }
}