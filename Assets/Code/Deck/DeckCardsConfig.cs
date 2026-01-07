using System.Collections.Generic;
using Game.Cards;
// using Lib;
using UnityEngine;

namespace Object.Deck
{
    [CreateAssetMenu(fileName = "Deck Cards Config", menuName = "Game/Deck Cards Config", order = 0)]
    public class DeckCardsConfig : ScriptableObject
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

        public CardParametrs GetCard( CardTypeEnum type, int index )
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

        public ICard GetRandomCard( CardTypeEnum type )
        {
            switch (type)
            {
                case CardTypeEnum.Attack:

                    int a = Random.Range(0, cardsAttack.Count);

                    ICard cardA = new DefaultCard()
                    {
                        Type = cardsAttack[a].Type,
                        CardName = cardsAttack[a].CardName,
                        Description = cardsAttack[a].Description,
                        Icon = cardsAttack[a].Icon,
                        Effects = cardsAttack[a].Effects
                    };

                    return new AttackDecorator(cardsAttack[a].DamageValue, cardA);
                
                case CardTypeEnum.Defence:
                
                    int d = Random.Range(0, cardsDefence.Count);

                    ICard cardD = new DefaultCard()
                    {
                        Type = cardsDefence[d].Type,
                        CardName = cardsDefence[d].CardName,
                        Description = cardsDefence[d].Description,
                        Icon = cardsDefence[d].Icon,
                        Effects = cardsDefence[d].Effects
                    };

                    return new HealthDecorator(cardsDefence[d].HealthValue, cardD);
                
                case CardTypeEnum.Monster:
                
                    int m = Random.Range(0, cardsMonster.Count);

                    ICard cardM = new DefaultCard()
                    {
                        Type = cardsMonster[m].Type,
                        CardName = cardsMonster[m].CardName,
                        Description = cardsMonster[m].Description,
                        Icon = cardsMonster[m].Icon,
                        Effects = cardsMonster[m].Effects
                    };

                    ICard cardMA = new AttackDecorator(cardsMonster[m].DamageValue, cardM);
                    return new HealthDecorator(cardsMonster[m].HealthValue, cardMA);

                default:
                    Debug.LogError("Такого типа карты не существует.");
                    return null;
            }
        }
    }
}