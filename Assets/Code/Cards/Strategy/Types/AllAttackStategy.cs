using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.Strategy
{
    #region Standart Attack Strategy

    public class StandartAttackStategy : StrategyAttackBase
    {
        public override string Name => "Default Attack";
        private GameObject target;
        private GameObject parent;
        private int currentDamageValue;
        private CardTypeEnum IgnoreLayers;

        public StandartAttackStategy(GameObject target, GameObject parent, int damageValue, CardTypeEnum ignoreLayers)
        {
            this.target = target;
            this.parent = parent;
            currentDamageValue = damageValue;
            this.IgnoreLayers = ignoreLayers;
        }

        public override void Execute()
        {
            CardData data = target.GetComponent<CardData>();

            if(data.TryGetCardFeature<HealthDecorator>(out var feature) && !data.decorateCard.IgnoreLayers.HasFlag(IgnoreLayers))
            {
                //! Удаление карты атаки при нанесении урона по карте монстра
                if(data.decorateCard.Type.HasFlag(CardTypeEnum.Monster))
                {
                    parent.GetComponent<CardData>().CardDestroy(false);
                }

                feature.TakeDamage(currentDamageValue);
            }
        }

        public override void UpdateTarget(GameObject newTarget)
        {
            target = newTarget;
        }

        public override void UpdateDamageValue(int newDamageValue)
        {
            currentDamageValue = newDamageValue;
        }
    }

    public class MonsterAttackStrategy : StrategyAttackBase
    {
        public override string Name => "Default Attack";
        private PlayerSystem player;
        private int currentDamageValue;
        private IDeck<CardData> defenceDeck;

        public MonsterAttackStrategy(PlayerSystem player, int damageValue, IDeck<CardData> defenceDeck)
        {
            this.player = player;
            currentDamageValue = damageValue;
            this.defenceDeck = defenceDeck;
        }

        public override void Execute()
        {
            if(defenceDeck.GetCardCount() > 0 )
            {
                foreach (var card in defenceDeck.cardDatas)
                {
                    if(card == null || card.decorateCard == null) continue;
                    if(card.TryGetCardFeature<HealthDecorator>(out var decorator))
                    {
                        decorator.TakeDamage(currentDamageValue);
                        break;
                    }
                }
            }
            else player.Kill();
        }

        public override void UpdateDamageValue(int newDamageValue)
        {
            currentDamageValue = newDamageValue;
        }
    }

    #endregion

    #region Modifiers Attack

    public class IgnoreDefenceTypeAttackStategy : StrategyAttackBase
    {
        public override string Name => "Ignore defence type attack";
        private int currentDamageValue;
        private IDeck<CardData> defendDeck;
        private PlayerSystem player;
        private DefenceType type;

        public IgnoreDefenceTypeAttackStategy(PlayerSystem player, DefenceType defenceType, int damageValue, IDeck<CardData> defendDeck)
        {
            currentDamageValue = damageValue;
            this.defendDeck = defendDeck;
            this.player = player;
            type = defenceType;
        }

        public override void Execute()
        {
            CardData defenceItem = null;

            if(defendDeck.GetCardCount() == 0) 
            {
                player.Kill();
                return;
            }

            foreach (var card in defendDeck.cardDatas)
            {
                if(card == null) continue;

                if (card.GetCardFeature<CustomTypeDecorator<DefenceType>>().CustomType != type)
                {
                    card.GetCardFeature<HealthDecorator>().TakeDamage(currentDamageValue);
                    return;
                }
                else defenceItem = card;
            }

            if(defenceItem != null) player.Kill();
        }
    }

    public class DeckStrikeAttackStrategy : StrategyAttackBase
    {
        public override string Name => "Default Attack";
        private PlayerSystem player;
        private int currentDamageValue;
        private IDeck<CardData> defenceDeck;
        private IDeck<CardData> monsterDeck;

        public DeckStrikeAttackStrategy(PlayerSystem player, int damageValue, IDeck<CardData> defenceDeck, IDeck<CardData> monsterDeck)
        {
            this.player = player;
            currentDamageValue = damageValue;
            this.defenceDeck = defenceDeck;
            this.monsterDeck = monsterDeck;
        }

        public override void Execute()
        {
            if(defenceDeck.GetCardCount() > 0 )
            {
                int multiply = 0;

                foreach (var card in defenceDeck.cardDatas)
                {
                    if(card == null || card.decorateCard == null) continue;
                    if(card.TryGetCardFeature<HealthDecorator>(out var decorator))
                    {
                        multiply++;
                        decorator.TakeDamage(currentDamageValue);
                        break;
                    }
                }

                foreach (var card in monsterDeck.cardDatas)
                {
                    // card.GetCardFeature<HealthDecorator>().healthSystem.H
                }
            }
            else player.Kill();
        }

        public override void UpdateDamageValue(int newDamageValue) => currentDamageValue = newDamageValue;
    }

    #endregion
}