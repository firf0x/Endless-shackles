using System.Collections.Generic;
using Game.Cards;
// using Lib;
using UnityEngine;

namespace Game.Cards
{
    [CreateAssetMenu(fileName = "AllCards", menuName = "Game/Configs/AllCards", order = 0)]
    public class CardsConfig : ScriptableObject
    {
        // * Здесь информация о игроке
        // [field: SerializeField] public CardPlayer playerData { get; private set; }

        // * Здесь задаются данные о картах.
        [SerializeField, Space(15)] private List<CardAttack> cardsAttack = new List<CardAttack>();
        [SerializeField, Space(15)] private List<CardDefence> cardsDefence = new List<CardDefence>();
        [SerializeField, Space(15)] private List<CardMonster> cardsMonster = new List<CardMonster>();

        // * Какие карты должны использоваться для создания карт. 
        
        [field: SerializeField, Space(10)] public GameObject prefabCardAttack { get; private set; }
        [field: SerializeField] public GameObject prefabCardDefence { get; private set; }
        [field: SerializeField] public GameObject prefabCardMonster { get; private set; }
        // [field: SerializeField] public GameObject prefabCardPlayer { get; private set; }

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