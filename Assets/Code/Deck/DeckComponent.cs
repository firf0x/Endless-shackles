using Game.Cards;
using Game.Deck;
using UnityEngine;

namespace Object.Deck
{
    public class DeckComponent : MonoBehaviour
    {
        [SerializeField] private DeckCards cards;
        [SerializeField] private HandDeck handDeck;
        private IDeck deck => handDeck;

        public void CreateNewCard()
        {
            CardTypeEnum typeEnum = (CardTypeEnum)Random.Range(0, (int)CardTypeEnum.Monster + 1);

            CardBase card = null;
            GameObject cardPrefab = null;

            switch (typeEnum)
            {
                case CardTypeEnum.Attack:
                    card = cards.GetRandomCard(typeEnum);
                    
                    cardPrefab = Instantiate(cards.prefabCardAttack);
                    break;
                
                case CardTypeEnum.Defence:
                    card = cards.GetRandomCard(typeEnum);
                    
                    cardPrefab = Instantiate(cards.prefabCardDefence);
                    break;
                
                case CardTypeEnum.Monster:
                    card = cards.GetRandomCard(typeEnum);
                    
                    cardPrefab = Instantiate(cards.prefabCardMonster);
                    break;

                default:
                    Debug.LogError("Такого типа карты не существует.");
                    break;
            }

            cardPrefab.GetComponent<CardData>().cardBase = card;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = card.Icon;

            deck.AddCard(cardPrefab.GetComponent<CardData>());
        }
    }
}