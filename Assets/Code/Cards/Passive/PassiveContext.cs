using Game.Lib;
using UnityEngine;

namespace Game.Cards.Passive
{
    public class PassiveContext
    {
        public readonly IDeck<CardData> HandDeck;
        public readonly IDeck<CardData> DefenceDeck;
        public readonly IDeck<CardData> MonsterDeck;

        public GameObject Parent;
        public GameObject Target;

        public PassiveContext(IDeck<CardData> hand, IDeck<CardData> defence, IDeck<CardData> monster)
        {
            HandDeck = hand;
            DefenceDeck = defence;
            MonsterDeck = monster;
        }
    }
}