using Game.Cards;
using Game.Deck.Fabric;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Deck
{
    public class DeckComponent : MonoBehaviour
    {
        [SerializeField] private DeckCardsConfig config;
        [field:SerializeField] public GameObject GameOverWindow { get; private set; } //! ГОВНО КОД

        [field:SerializeField] public HandDeck handDeck { get; private set; }
        [field:SerializeField] public DefendDeck defenceDeck { get; private set; }
        [field:SerializeField] public MonsterDeck monsterDeck { get; private set; }

        private Creator<ICard<CardTypeEnum>> creator;
        [SerializeField] private PlayerSystem playerSystem = new();

        private void Awake()
        {
            creator = new CardCreator(config, handDeck, defenceDeck, monsterDeck, playerSystem);
        }

        public void CreateNewCard()
        {
            ICard<CardTypeEnum> card = creator.Create();
            
            GameObject cardPrefab = null;

            StepCombatSystem.Instance.StepUpdate();

            switch (card.Type)
            {
                case CardTypeEnum.Attack:
                    cardPrefab = Instantiate(config.prefabCardAttack, transform);
                    handDeck.AddCard(cardPrefab.GetComponent<CardData>());
                    cardPrefab.GetComponent<CardData>().currentDeck = handDeck;
                    break;
                
                case CardTypeEnum.Defence:
                    cardPrefab = Instantiate(config.prefabCardDefence, transform);
                    handDeck.AddCard(cardPrefab.GetComponent<CardData>());
                    cardPrefab.GetComponent<CardData>().currentDeck = handDeck;
                    break;
                
                case CardTypeEnum.Monster:
                    // if(monsterDeck.is) break;
                    cardPrefab = Instantiate(config.prefabCardMonster, transform);
                    monsterDeck.AddCard(cardPrefab.GetComponent<CardData>());
                    cardPrefab.GetComponent<CardData>().currentDeck = monsterDeck;
                    break;

                default:
                    Debug.LogError("Такого типа карты не существует.");
                    break;
            }

            card.Parent = cardPrefab;
            cardPrefab.GetComponent<CardData>().decorateCard = card;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = card.Icon;

            card.Start();
        }

        public void CreateAttackCard()
        {
            ICard<CardTypeEnum> card = creator.CreateAttackCard();
            GameObject cardPrefab = Instantiate(config.prefabCardAttack, transform);
            handDeck.AddCard(cardPrefab.GetComponent<CardData>());
            cardPrefab.GetComponent<CardData>().currentDeck = handDeck;

            card.Parent = cardPrefab;
            cardPrefab.GetComponent<CardData>().decorateCard = card;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = card.Icon;

            card.Start();
        }

        public void CreateDefenceCard()
        {
            ICard<CardTypeEnum> card = creator.CreateDefenceCard();
            GameObject cardPrefab = Instantiate(config.prefabCardDefence, transform);
            handDeck.AddCard(cardPrefab.GetComponent<CardData>());
            cardPrefab.GetComponent<CardData>().currentDeck = handDeck;

            card.Parent = cardPrefab;
            cardPrefab.GetComponent<CardData>().decorateCard = card;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = card.Icon;

            card.Start();            
        }

        public void ResetDatas()
        {
            handDeck.ClearCards();
            defenceDeck.ClearCards();
            monsterDeck.ClearCards();
            playerSystem.Respawn();
        }

        public void OnDestroy()
        {
            playerSystem.Dispose();
        }
    }
}