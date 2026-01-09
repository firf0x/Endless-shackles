using Game.Cards;
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

        private IPool<ICard<CardTypeEnum>> pool;
        private IDeck deck => handDeck;

        private void Awake()
        {
            // pool = new PoolCard(config);
        }

        public void CreateNewCard()
        {
            ICard<CardTypeEnum> card = pool.Get(transform);
            
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
            // pool.Dispose();
        }
    }
}