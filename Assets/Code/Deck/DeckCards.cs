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
    }
}