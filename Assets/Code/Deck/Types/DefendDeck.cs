using Game.Cards;
using Game.Lib;
using UnityEngine;

namespace Game.Deck
{
    public sealed class DefendDeck : MonoBehaviour, IDeck<CardData>
    {
        public CardData[] cardDatas { get; private set;}



        public void AddCard(CardData newCard)
        {
            
        }

        public void RemoveCard(CardData deletedCard)
        {
            
        }
    }
}