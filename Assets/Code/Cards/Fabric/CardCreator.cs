using System;
using System.Collections.Generic;
using Game.Cards.Passive;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Fabric
{
    public class CardCreator : Creator<ICard>
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

        public override ICard Create()
        {
            int p = UnityEngine.Random.Range(0, 3);
            CardTypeEnum cardType = (CardTypeEnum)(1 << p);

            return CreateRandomCard(cardType, null);
        }

        public override ICard Create(Transform transform)
        {
            int p = UnityEngine.Random.Range(0, 3);
            CardTypeEnum cardType = (CardTypeEnum)(1 << p);

            return CreateRandomCard(cardType, transform);
        }

        public override ICard CreateAttackCard() => CreateAttackCard(UnityEngine.Random.Range(0, config.CardsAttacks.Count), null);
        public override ICard CreateDefenceCard() => CreateDefenceCard(UnityEngine.Random.Range(0, config.CardsDefence.Count), null);


        private ICard CreateRandomCard(CardTypeEnum typeEnum, Transform transform)
        {
            ICard card = null;

            switch (typeEnum)
            {
                case CardTypeEnum.Attack: card = CreateAttackCard(UnityEngine.Random.Range(0, config.CardsAttacks.Count), transform); break;
                case CardTypeEnum.Defence: card = CreateDefenceCard(UnityEngine.Random.Range(0, config.CardsDefence.Count), transform); break;
                case CardTypeEnum.Monster: card = CreateMonsterCard(UnityEngine.Random.Range(0, config.CardsMonster.Count), transform); break;
            }

            return card;
        }

        private ICard CreateAttackCard(int index, Transform transform)
        {
            var cardsAttack = config.CardsAttacks;

            CardAttack cardData = cardsAttack[index];

            GameObject gameObject = GameObject.Instantiate(config.prefabCardAttack);
            if(transform != null) gameObject.transform.position = transform.position;

            ICard baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                InteractionLayers = cardData.IgnoreLayers,
                CardName = cardData.Name,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Parent = gameObject,
                CardData = gameObject.GetComponent<CardData>()
            };
            
            // Инициализация Monobehaviour
            baseCard.CardData.currentDeck = handDeck;
            baseCard.CardData.ObjectRenderer.sprite = baseCard.Icon;

            List<ModifierBase> modifiers = new List<ModifierBase>();

            foreach (var modifier in cardData.Modifiers)
            {
                if(modifier != null)
                {
                    var obj = ScriptableObject.Instantiate(modifier);
                    modifiers.Add(obj);
                }
            }

            List<PassivesBase> passives = new List<PassivesBase>(cardData.Passives);

            ICard modifierCard = new ModifierDecorator(modifiers, handDeck, defendDeck, monsterDeck, baseCard);
            ICard passivesCard = new PassiveDecorator(passives, handDeck, defendDeck, monsterDeck, modifierCard);
            ICard callBackCard = new CallBackDecorator(passivesCard);
            ICard attackCard = new AttackDecorator(cardData.DamageValue, cardData.strategyAttack, defendDeck, callBackCard);

            baseCard.Parent.GetComponent<CardData>().decorateCard = attackCard;
            handDeck.AddCard(baseCard.Parent.GetComponent<CardData>());

            return attackCard;
        }

        private ICard CreateDefenceCard(int index, Transform transform)
        {
            var cardsDefence = config.CardsDefence;

            CardDefence cardData = cardsDefence[index];

            GameObject gameObject = GameObject.Instantiate(config.prefabCardDefence);
            if(transform != null) gameObject.transform.position = transform.position;

            ICard baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                InteractionLayers = cardData.IgnoreLayers,
                CardName = cardData.Name,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Parent = gameObject,
                CardData = gameObject.GetComponent<CardData>()
            };
            
            // Инициализация Monobehaviour
            baseCard.CardData.currentDeck = handDeck;
            baseCard.CardData.ObjectRenderer.sprite = baseCard.Icon;
            
            List<ModifierBase> modifiers = new List<ModifierBase>();

            foreach (var modifier in cardData.Modifiers)
            {
                if(modifier != null)
                {
                    modifiers.Add(ScriptableObject.Instantiate(modifier));
                    Debug.Log(modifier.name);
                }
            }

            List<PassivesBase> passives = new List<PassivesBase>(cardData.Passives);

            ICard modifierCard = new ModifierDecorator(modifiers, handDeck, defendDeck, monsterDeck, baseCard);
            ICard callBackCard = new CallBackDecorator(modifierCard);
            ICard passivesCard = new PassiveDecorator(passives, handDeck, defendDeck, monsterDeck, callBackCard);
            ICard defenceType = new CustomTypeDecorator<DefenceType>(cardData.DefenceType, passivesCard);
            ICard healthCard = new HealthDecorator(cardData.HealthValue, defenceType);

            baseCard.Parent.GetComponent<CardData>().decorateCard = healthCard;
            handDeck.AddCard(baseCard.Parent.GetComponent<CardData>());

            return healthCard;
        }

        private ICard CreateMonsterCard(int index, Transform transform)
        {
            var cardsMonster = config.CardsMonster;

            CardMonster cardData = cardsMonster[index];

            GameObject gameObject = GameObject.Instantiate(config.prefabCardMonster);
            if(transform != null) gameObject.transform.position = transform.position;

            ICard baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                InteractionLayers = cardData.IgnoreLayers,
                CardName = cardData.Name,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Parent = gameObject,
                CardData = gameObject.GetComponent<CardData>()
            };
            
            // Инициализация Monobehaviour
            baseCard.CardData.currentDeck = monsterDeck;
            baseCard.CardData.ObjectRenderer.sprite = baseCard.Icon;

            List<ModifierBase> modifiers = new List<ModifierBase>();
            
            foreach (var modifier in cardData.Modifiers)
            {
                if(modifier != null)
                {
                    var obj = ScriptableObject.Instantiate(modifier);
                    modifiers.Add(obj);
                }
            }
            
            List<PassivesBase> passives = new List<PassivesBase>(cardData.Passives);

            ICard modifierCard = new ModifierDecorator(modifiers, handDeck, defendDeck, monsterDeck, baseCard);
            ICard callBackCard = new CallBackDecorator(modifierCard);
            ICard passivesCard = new PassiveDecorator(passives, handDeck, defendDeck, monsterDeck, callBackCard);
            ICard attackCard = new AttackDecorator(cardData.DamageValue, cardData.strategyAttack, defendDeck, passivesCard);
            ICard stepCard = new StepCombatDecorator(cardData.StepValue, attackCard);
            ICard healthCard = new HealthDecorator(cardData.HealthValue, stepCard);
            
            baseCard.Parent.GetComponent<CardData>().decorateCard = healthCard;
            monsterDeck.AddCard(baseCard.Parent.GetComponent<CardData>());
            
            return healthCard;
        }
    }
}