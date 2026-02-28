using System;
using System.Collections.Generic;
using Game.Cards;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Deck.Fabric
{
    public class CardCreator : Creator<ICard<CardTypeEnum>>
    {
        private readonly DeckCardsConfig config;
        private readonly PlayerSystem player;
        private readonly IDeck<CardData> handDeck;
        private readonly IDeck<CardData> defendDeck;
        private readonly IDeck<CardData> monsterDeck;
        public CardCreator(DeckCardsConfig config, IDeck<CardData> handDeck, IDeck<CardData> defendDeck, IDeck<CardData> monsterDeck, PlayerSystem player)
        {
            this.config = config;
            this.player = player;
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

        private ICard<CardTypeEnum> CreateRandomCard(CardTypeEnum typeEnum)
        {
            ICard<CardTypeEnum> card = null;

            switch (typeEnum)
            {
                case CardTypeEnum.Attack: card = CreateAttackCard(UnityEngine.Random.Range(0, config.CardsAttacks.Count)); break;
                case CardTypeEnum.Defence: card = CreateDefenceCard(UnityEngine.Random.Range(0, config.CardsDefence.Count)); break;
                case CardTypeEnum.Monster: card = CreateMonsterCard(UnityEngine.Random.Range(0, config.CardsMonster.Count)); break;

                default: card = null; break;

            }

            return card;
        }

        private ICard<CardTypeEnum> CreateAttackCard(int index)
        {
            var cardsAttack = config.CardsAttacks;

            var cardData = cardsAttack[index];

            ICard<CardTypeEnum> baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                IgnoreLayers = cardData.IgnoreLayers,
                CardName = cardData.CardName,
                Description = cardData.Description,
                Icon = cardData.Icon,
            };
            
            List<ModifierBase> modifiers = new List<ModifierBase>(cardData.Modifiers);

            ICard<CardTypeEnum> modifierCard = new ModifierDecorator(modifiers, handDeck, defendDeck, monsterDeck, player, baseCard);
            ICard<CardTypeEnum> callBackCard = new CallBackDecorator(modifierCard);

            return new AttackDecorator(cardData.DamageValue, player, defendDeck, callBackCard);
        }

        private ICard<CardTypeEnum> CreateDefenceCard(int index)
        {
            var cardsDefence = config.CardsDefence;

            var cardData = cardsDefence[index];

            ICard<CardTypeEnum> baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                IgnoreLayers = cardData.IgnoreLayers,
                CardName = cardData.CardName,
                Description = cardData.Description,
                Icon = cardData.Icon,
            };
            
            ICard<CardTypeEnum> defenceType = new CustomTypeDecorator<DefenceType>(cardData.DefenceType, baseCard);

            return new HealthDecorator(cardData.HealthValue, defenceType);
        }

        private ICard<CardTypeEnum> CreateMonsterCard(int index)
        {
            var cardsMonster = config.CardsMonster;

            var cardData = cardsMonster[index];

            ICard<CardTypeEnum> baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                IgnoreLayers = cardData.IgnoreLayers,
                CardName = cardData.CardName,
                Description = cardData.Description,
                Icon = cardData.Icon,
            };

            List<ModifierBase> modifiers = new List<ModifierBase>(cardData.Modifiers);

            ICard<CardTypeEnum> modifierCard = new ModifierDecorator(modifiers, handDeck, defendDeck, monsterDeck, player, baseCard);
            ICard<CardTypeEnum> callBackCard = new CallBackDecorator(modifierCard);
            ICard<CardTypeEnum> cardWithAttack = new AttackDecorator(cardData.DamageValue, player, defendDeck, callBackCard);
            ICard<CardTypeEnum> cardWithHealth = new HealthDecorator(cardData.HealthValue, cardWithAttack);

            return new StepCombatDecorator(cardData.StepValue, cardWithHealth);
        }
    }
}