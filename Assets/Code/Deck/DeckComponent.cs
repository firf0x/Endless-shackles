using Game.Cards;
using Game.Deck.Fabric;
using Game.Lib;
using UnityEngine;

namespace Game.Deck
{
    public class DeckComponent : MonoBehaviour
    {
        [SerializeField] private DeckCardsConfig config;
        [SerializeField] private HandDeck handDeck;
        [SerializeField] private DefendDeck defenceDeck;
        [SerializeField] private MonsterDeck monsterDeck;

        private Creator<ICard<CardTypeEnum>> creator;
        // private StepCombatSystem;

        private void Awake()
        {
            var localCreator = new CardCreator(config);
            creator = localCreator;

            defenceDeck.player.GetComponent<CardData>().decorateCard = localCreator.CreatePlayerCard();
        }

        public void CreateNewCard()
        {
            ICard<CardTypeEnum> card = creator.Create();
            
            GameObject cardPrefab = null;

            switch (card.Type)
            {
                case CardTypeEnum.Attack:
                    cardPrefab = Instantiate(config.prefabCardAttack, transform);
                    handDeck.AddCard(cardPrefab.GetComponent<CardData>());
                    break;
                
                case CardTypeEnum.Defence:
                    cardPrefab = Instantiate(config.prefabCardDefence, transform);
                    handDeck.AddCard(cardPrefab.GetComponent<CardData>());
                    break;
                
                case CardTypeEnum.Monster:
                    cardPrefab = Instantiate(config.prefabCardMonster, transform);
                    monsterDeck.AddCard(cardPrefab.GetComponent<CardData>());
                    break;

                default:
                    Debug.LogError("Такого типа карты не существует.");
                    break;
            }

            card.Parent = cardPrefab;
            cardPrefab.GetComponent<CardData>().decorateCard = card;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = card.Icon;




            // deck.AddCard(cardPrefab.GetComponent<CardData>());
        }
    }
}