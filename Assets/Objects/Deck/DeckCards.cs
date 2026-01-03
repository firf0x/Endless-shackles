using System.Collections.Generic;
using Object.Cards;
using UnityEngine;

namespace Object.Deck
{
    [CreateAssetMenu(fileName = "DeckCard", menuName = "Game/DeckCard", order = 0)]
    public class DeckCards : ScriptableObject
    {
        [SerializeField] private List<CardBase> cards = new List<CardBase>();
    }
}