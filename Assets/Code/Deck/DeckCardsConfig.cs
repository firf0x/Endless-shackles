using System.Collections.Generic;
using Game.Cards;
// using Lib;
using UnityEngine;

namespace Game.Deck
{
    [CreateAssetMenu(fileName = "Deck Cards Config", menuName = "Game/Deck Cards Config", order = 0)]
    public class DeckCardsConfig : ScriptableObject
    {
        // * Здесь задаются данные о картах.
        [SerializeField] private List<CardAttack> cardsAttack = new List<CardAttack>();
        [SerializeField] private List<CardDefence> cardsDefence = new List<CardDefence>();
        [SerializeField] private List<CardMonster> cardsMonster = new List<CardMonster>();

        // * Какие карты должны использоваться для создания карт. 
        [SerializeField] public GameObject prefabCardAttack;
        [SerializeField] public GameObject prefabCardDefence;
        [SerializeField] public GameObject prefabCardMonster;

        public int CountCards => cardsAttack.Count + cardsDefence.Count + cardsMonster.Count;

        public int CountCardsAttack => cardsAttack.Count;
        public int CountCardsDefence => cardsDefence.Count;
        public int CountCardsMonster => cardsDefence.Count;

        // * Списки для работы с картами
        public IReadOnlyList<CardAttack> CardsAttacks => cardsAttack;
        public IReadOnlyList<CardDefence> CardsDefence => cardsDefence;
        public IReadOnlyList<CardMonster> CardsMonster => cardsMonster;
    }
}