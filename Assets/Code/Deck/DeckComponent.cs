using Game.Cards;
using Game.Cards.Fabric;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Deck
{
    public class DeckComponent : MonoBehaviour
    {
        [SerializeField] private CardsConfig config;
        [field:SerializeField] public GameObject GameOverWindow { get; private set; } //! ГОВНО КОД

        [field:SerializeField] public HandDeck handDeck { get; private set; }
        [field:SerializeField] public DefendDeck defenceDeck { get; private set; }
        [field:SerializeField] public MonsterDeck monsterDeck { get; private set; }

        private Creator<ICard> creator;
        [SerializeField] private PlayerSystem playerSystem = new();

        private void Awake()
        {
            creator = new CardCreator(config, handDeck, defenceDeck, monsterDeck);
        }

        public void CreateNewCard(Transform transform)
        {
            StepCombatSystem.Instance.StepUpdate();
            
            ICard card = creator.Create(transform);
            
            card.Start();
        }

        public void CreateAttackCard()
        {
            ICard card = creator.CreateAttackCard();

            card.Start();
        }

        public void CreateDefenceCard()
        {
            ICard card = creator.CreateDefenceCard();

            card.Start();            
        }

        // Тест "Убрать"
        public void NextStep()
        {
            StepCombatSystem.Instance.StepUpdate();
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