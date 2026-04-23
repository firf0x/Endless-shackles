using System;
using System.Collections.Generic;
using Game.Cards.Passive;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Fabric
{
    public class CardCreator : Creator<ICard<CardTypeEnum>>
    {
        private readonly CardsConfig config;
        private readonly IDeck<CardData> handDeck;
        private readonly IDeck<CardData> defendDeck;
        private readonly IDeck<CardData> monsterDeck;

        public CardCreator(CardsConfig config, IDeck<CardData> handDeck, IDeck<CardData> defendDeck, IDeck<CardData> monsterDeck)
        {
            this.config = config;
            this.handDeck = handDeck;
            this.defendDeck = defendDeck;
            this.monsterDeck = monsterDeck;
        }

        public override ICard<CardTypeEnum> Create()
        {
            int p = UnityEngine.Random.Range(0, 3);
            CardTypeEnum cardType = (CardTypeEnum)(1 << p);

            return CreateRandomCard(cardType);
        }

        public override ICard<CardTypeEnum> CreateAttackCard() => CreateAttackCard(UnityEngine.Random.Range(0, config.CardsAttacks.Count));
        public override ICard<CardTypeEnum> CreateDefenceCard() => CreateDefenceCard(UnityEngine.Random.Range(0, config.CardsDefence.Count));


        private ICard<CardTypeEnum> CreateRandomCard(CardTypeEnum typeEnum)
        {
            ICard<CardTypeEnum> card = null;

            switch (typeEnum)
            {
                case CardTypeEnum.Attack: card = CreateAttackCard(UnityEngine.Random.Range(0, config.CardsAttacks.Count)); break;
                case CardTypeEnum.Defence: card = CreateDefenceCard(UnityEngine.Random.Range(0, config.CardsDefence.Count)); break;
                case CardTypeEnum.Monster: card = CreateMonsterCard(UnityEngine.Random.Range(0, config.CardsMonster.Count)); break;
            }

            return card;
        }

        private ICard<CardTypeEnum> CreateAttackCard(int index)
        {
            var cardsAttack = config.CardsAttacks;

            CardAttack cardData = cardsAttack[index];

            ICard<CardTypeEnum> baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                IgnoreLayers = cardData.IgnoreLayers,
                CardName = cardData.Name,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Parent = GameObject.Instantiate(config.prefabCardAttack)
            };
            
            // Инициализация Monobehaviour
            baseCard.Parent.GetComponent<CardData>().currentDeck = handDeck;
            baseCard.Parent.GetComponent<CardData>().ObjectRenderer.sprite = baseCard.Icon;

            List<ModifierBase> modifiers = new List<ModifierBase>(cardData.Modifiers);
            List<PassivesBase> passives = new List<PassivesBase>(cardData.strategyPassive);

            ICard<CardTypeEnum> modifierCard = new ModifierDecorator(modifiers, handDeck, defendDeck, monsterDeck, baseCard);
            ICard<CardTypeEnum> callBackCard = new CallBackDecorator(modifierCard);
            ICard<CardTypeEnum> passivesCard = new PassiveDecorator(passives, handDeck, defendDeck, monsterDeck, callBackCard);
            ICard<CardTypeEnum> attackCard = new AttackDecorator(cardData.DamageValue, cardData.strategyAttack, defendDeck, passivesCard);

            baseCard.Parent.GetComponent<CardData>().decorateCard = attackCard;
            handDeck.AddCard(baseCard.Parent.GetComponent<CardData>());

            return attackCard;
        }

        private ICard<CardTypeEnum> CreateDefenceCard(int index)
        {
            var cardsDefence = config.CardsDefence;

            CardDefence cardData = cardsDefence[index];

            ICard<CardTypeEnum> baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                IgnoreLayers = cardData.IgnoreLayers,
                CardName = cardData.Name,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Parent = GameObject.Instantiate(config.prefabCardDefence)
            };
            
            // Инициализация Monobehaviour
            baseCard.Parent.GetComponent<CardData>().currentDeck = handDeck;
            baseCard.Parent.GetComponent<CardData>().ObjectRenderer.sprite = baseCard.Icon;
            
            List<ModifierBase> modifiers = new List<ModifierBase>(cardData.Modifiers);
            List<PassivesBase> passives = new List<PassivesBase>(cardData.strategyPassive);

            ICard<CardTypeEnum> modifierCard = new ModifierDecorator(modifiers, handDeck, defendDeck, monsterDeck, baseCard);
            ICard<CardTypeEnum> callBackCard = new CallBackDecorator(modifierCard);
            ICard<CardTypeEnum> passivesCard = new PassiveDecorator(passives, handDeck, defendDeck, monsterDeck, callBackCard);
            ICard<CardTypeEnum> defenceType = new CustomTypeDecorator<DefenceType>(cardData.DefenceType, passivesCard);
            ICard<CardTypeEnum> healthCard = new HealthDecorator(cardData.HealthValue, defenceType);

            baseCard.Parent.GetComponent<CardData>().decorateCard = healthCard;
            handDeck.AddCard(baseCard.Parent.GetComponent<CardData>());

            return healthCard;
        }

        private ICard<CardTypeEnum> CreateMonsterCard(int index)
        {
            var cardsMonster = config.CardsMonster;

            CardMonster cardData = cardsMonster[index];

            ICard<CardTypeEnum> baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                IgnoreLayers = cardData.IgnoreLayers,
                CardName = cardData.Name,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Parent = GameObject.Instantiate(config.prefabCardMonster)
            };
            
            // Инициализация Monobehaviour
            baseCard.Parent.GetComponent<CardData>().currentDeck = monsterDeck;
            baseCard.Parent.GetComponent<CardData>().ObjectRenderer.sprite = baseCard.Icon;

            List<ModifierBase> modifiers = new List<ModifierBase>(cardData.Modifiers);
            List<PassivesBase> passives = new List<PassivesBase>(cardData.strategyPassive);

            ICard<CardTypeEnum> modifierCard = new ModifierDecorator(modifiers, handDeck, defendDeck, monsterDeck, baseCard);
            ICard<CardTypeEnum> callBackCard = new CallBackDecorator(modifierCard);
            ICard<CardTypeEnum> passivesCard = new PassiveDecorator(passives, handDeck, defendDeck, monsterDeck, callBackCard);
            ICard<CardTypeEnum> attackCard = new AttackDecorator(cardData.DamageValue, cardData.strategyAttack, defendDeck, passivesCard);
            ICard<CardTypeEnum> stepCard = new StepCombatDecorator(cardData.StepValue, attackCard);
            ICard<CardTypeEnum> healthCard = new HealthDecorator(cardData.HealthValue, stepCard);
            
            baseCard.Parent.GetComponent<CardData>().decorateCard = healthCard;
            monsterDeck.AddCard(baseCard.Parent.GetComponent<CardData>());
            
            return healthCard;
        }
    }
}