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
        [SerializeField] private HandDeck defenceDeck;
        [SerializeField] private HandDeck monsterDeck;

        private Creator<ICard<CardTypeEnum>> creator;
        private IDeck<CardData> deck => handDeck;

        private void Awake()
        {
            creator = new CardCreator(config);
        }

        public void CreateNewCard()
        {
            ICard<CardTypeEnum> card = creator.Create();
            
            GameObject cardPrefab = null;

            switch (card.Type)
            {
                case CardTypeEnum.Attack:
                    cardPrefab = Instantiate(config.prefabCardAttack, transform);
                    break;
                
                case CardTypeEnum.Defence:
                    cardPrefab = Instantiate(config.prefabCardDefence, transform);
                    break;
                
                case CardTypeEnum.Monster:
                    cardPrefab = Instantiate(config.prefabCardMonster, transform);
                    break;

                default:
                    Debug.LogError("Такого типа карты не существует.");
                    break;
            }            

            card.Parent = cardPrefab;
            cardPrefab.GetComponent<CardData>().decorateCard = card;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = card.Icon;


            deck.AddCard(cardPrefab.GetComponent<CardData>());
        }
    }
}