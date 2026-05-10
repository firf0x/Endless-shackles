using System;
using System.Collections.Generic;
using Game.Cards;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Deck.Fabric
{
    // Для карт со стратегией по созданию миньонов
    [Serializable]
    public class MinionsCreator : Creator<ICard>
    {
        [SerializeField] private CardsConfig config;
        [SerializeField] private int MinWeight;

        [HideInInspector] public IDeck<CardData> handDeck;
        [HideInInspector] public IDeck<CardData> defenceDeck;
        [HideInInspector] public IDeck<CardData> monsterDeck;

        public override ICard Create()
        {
            var eligibleIndices = new List<int>();
            var eligibleWeights = new List<int>();

            for (int i = 0; i < config.CardsMonster.Count; i++)
            {
                var card = config.CardsMonster[i];
                
                if (card.Weight > MinWeight)
                {
                    eligibleIndices.Add(i);
                    eligibleWeights.Add(card.Weight);
                }
            }

            if (eligibleIndices.Count == 0)
            {
                Debug.LogWarning($"Нет карт с весом > {MinWeight}. Берём первую карту из списка.");
                return CreateMonsterMinionsCard(0, GameObject.Instantiate(config.prefabCardMonster));
            }

            int totalWeight = 0;
            foreach (int w in eligibleWeights) totalWeight += w;

            float randomPoint = UnityEngine.Random.Range(0f, totalWeight);

            int accumulated = 0;
            int selectedIdx = 0;
            
            for (int i = 0; i < eligibleWeights.Count; i++)
            {
                accumulated += eligibleWeights[i];
                if (randomPoint < accumulated)
                {
                    selectedIdx = i;
                    break;
                }
            }

            int originalIndex = eligibleIndices[selectedIdx];

            GameObject cardPrefab = GameObject.Instantiate(config.prefabCardMonster);

            var cardMinion = CreateMonsterMinionsCard(originalIndex, cardPrefab);

            cardPrefab.GetComponent<CardData>().decorateCard = cardMinion;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = cardMinion.Icon;
            cardPrefab.GetComponent<CardData>().currentDeck = monsterDeck;
            
            monsterDeck.AddCard(cardPrefab.GetComponent<CardData>());

            cardMinion.Start();

            return cardMinion;
        }

        public override ICard Create(Transform transform)
        {
            var eligibleIndices = new List<int>();
            var eligibleWeights = new List<int>();

            for (int i = 0; i < config.CardsMonster.Count; i++)
            {
                var card = config.CardsMonster[i];
                
                if (card.Weight > MinWeight)
                {
                    eligibleIndices.Add(i);
                    eligibleWeights.Add(card.Weight);
                }
            }

            if (eligibleIndices.Count == 0)
            {
                Debug.LogWarning($"Нет карт с весом > {MinWeight}. Берём первую карту из списка.");
                return CreateMonsterMinionsCard(0, GameObject.Instantiate(config.prefabCardMonster, transform));
            }

            int totalWeight = 0;
            foreach (int w in eligibleWeights) totalWeight += w;

            float randomPoint = UnityEngine.Random.Range(0f, totalWeight);

            int accumulated = 0;
            int selectedIdx = 0;
            
            for (int i = 0; i < eligibleWeights.Count; i++)
            {
                accumulated += eligibleWeights[i];
                if (randomPoint < accumulated)
                {
                    selectedIdx = i;
                    break;
                }
            }

            int originalIndex = eligibleIndices[selectedIdx];

            GameObject cardPrefab = GameObject.Instantiate(config.prefabCardMonster);
            if(transform != null) cardPrefab.transform.position = transform.position;
            
            var cardMinion = CreateMonsterMinionsCard(originalIndex, cardPrefab);

            cardPrefab.GetComponent<CardData>().decorateCard = cardMinion;
            cardPrefab.GetComponent<CardData>().ObjectRenderer.sprite = cardMinion.Icon;
            cardPrefab.GetComponent<CardData>().currentDeck = monsterDeck;
            
            monsterDeck.AddCard(cardPrefab.GetComponent<CardData>());

            cardMinion.Start();

            return cardMinion;
        }

        private ICard CreateMonsterMinionsCard(int index, GameObject parent)
        {
            var cardsMonster = config.CardsMonster;

            var cardData = cardsMonster[index];

            ICard baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                InteractionLayers = cardData.IgnoreLayers,
                CardName = cardData.Name,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Parent = parent
            };

            List<ModifierBase> modifiers = new List<ModifierBase>(cardData.Modifiers);

            ICard modifierCard = new ModifierDecorator(modifiers, handDeck, defenceDeck, monsterDeck, baseCard);
            ICard callBackCard = new CallBackDecorator(modifierCard);
            ICard cardWithAttack = new AttackDecorator(cardData.DamageValue, cardData.strategyAttack, defenceDeck, callBackCard);
            ICard cardWithHealth = new HealthDecorator(cardData.HealthValue, cardWithAttack);

            return new StepCombatDecorator(cardData.StepValue, cardWithHealth);
        }
    }
}