using Game.Cards;
using Lib;
using UnityEngine;

namespace Game.Deck
{
    public class CardCreator : Creator<ICard>
    {
        private readonly DeckCardsConfig config;

        public CardCreator(DeckCardsConfig config) => this.config = config;

        public override ICard Create()
        {
            CardTypeEnum cardType = (CardTypeEnum)Random.Range(0, (int)CardTypeEnum.Monster + 1);

            return CreateRandomCard(cardType);
        }

        private ICard CreateRandomCard(CardTypeEnum typeEnum)
        {
            switch (typeEnum)
            {
                case CardTypeEnum.Attack: return CreateAttackCard(Random.Range(0, config.CardsAttacks.Count));
                case CardTypeEnum.Defence: return CreateDefenceCard(Random.Range(0, config.CardsDefence.Count));
                case CardTypeEnum.Monster: return CreateMonsterCard(Random.Range(0, config.CardsMonster.Count));

                default: return null;
            }
        }


        private ICard CreateAttackCard(int index)
        {
            var cardsAttack = config.CardsAttacks;

            var cardData = cardsAttack[index];

            ICard baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                CardName = cardData.CardName,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Effects = cardData.Effects
            };

            return new AttackDecorator(cardData.DamageValue, baseCard);
        }

        private ICard CreateDefenceCard(int index)
        {
            var cardsDefence = config.CardsDefence;

            var cardData = cardsDefence[index];

            ICard baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                CardName = cardData.CardName,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Effects = cardData.Effects
            };

            return new HealthDecorator(cardData.HealthValue, baseCard);
        }

        private ICard CreateMonsterCard(int index)
        {
            var cardsMonster = config.CardsMonster;

            var cardData = cardsMonster[index];

            ICard baseCard = new DefaultCard()
            {
                Type = cardData.Type,
                CardName = cardData.CardName,
                Description = cardData.Description,
                Icon = cardData.Icon,
                Effects = cardData.Effects
            };

            ICard cardWithAttack = new AttackDecorator(cardData.DamageValue, baseCard);

            return new HealthDecorator(cardData.HealthValue, cardWithAttack);
        }
    }
}