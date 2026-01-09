using Game.Cards;
using UnityEngine;

namespace Game.Deck
{
    public class DeckComponent : MonoBehaviour
    {
        [SerializeField] private DeckCardsConfig config;
        [SerializeField] private HandDeck handDeck;
        [SerializeField] private HandDeck defenceDeck;
        [SerializeField] private HandDeck monsterDeck;

        private PoolCard pool;

        private CardCreator fabric;

        private IDeck deck => handDeck;

        private void Awake()
        {
            fabric = new CardCreator(config);
            pool = new PoolCard(() => fabric.Create());
        }

        public void CreateNewCard()
        {
            ICard card = pool.Get(transform);
            
            GameObject cardPrefab = null;

            switch (card.Type)
            {
                case CardTypeEnum.Attack:
                    cardPrefab = Instantiate(config.prefabCardAttack);
                    break;
                
                case CardTypeEnum.Defence:
                    cardPrefab = Instantiate(config.prefabCardDefence);
                    break;
                
                case CardTypeEnum.Monster:
                    cardPrefab = Instantiate(config.prefabCardMonster);
                    break;

                default:
                    Debug.LogError("Такого типа карты не существует.");
                    break;
            }            

            cardPrefab.GetComponent<CardData>().decorateCard = card;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = card.Icon;

            deck.AddCard(cardPrefab.GetComponent<CardData>());
        }

        private void OnDestroy()
        {
            pool.Dispose();
        }
    }
}