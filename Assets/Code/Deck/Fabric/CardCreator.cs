using Game.Cards;
using Game.Lib;
using UnityEngine;

namespace Game.Deck.Fabric
{
    public class CardCreator : Creator<ICard<CardTypeEnum>>
    {
        private readonly DeckCardsConfig config;

        public CardCreator(DeckCardsConfig config) => this.config = config;

        public override ICard<CardTypeEnum> Create()
        {
            int p = Random.Range(0, 3);
            CardTypeEnum cardType = (CardTypeEnum)(1 << p);

            return CreateRandomCard(cardType);
        }

        private ICard<CardTypeEnum> CreateRandomCard(CardTypeEnum typeEnum)
        {
            ICard<CardTypeEnum> card = null;

            switch (typeEnum)
            {
                case CardTypeEnum.Attack: card = CreateAttackCard(Random.Range(0, config.CardsAttacks.Count)); break;
                case CardTypeEnum.Defence: card = CreateDefenceCard(Random.Range(0, config.CardsDefence.Count)); break;
                case CardTypeEnum.Monster: card = CreateMonsterCard(Random.Range(0, config.CardsMonster.Count)); break;

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
                Effects = cardData.Effects
            };

            return new AttackDecorator(cardData.DamageValue, baseCard);
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
                Effects = cardData.Effects
            };

            return new HealthDecorator(cardData.HealthValue, baseCard);
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
                Effects = cardData.Effects
            };

            ICard<CardTypeEnum> cardWithAttack = new AttackDecorator(cardData.DamageValue, baseCard);

            return new HealthDecorator(cardData.HealthValue, cardWithAttack);
        }
    }
}