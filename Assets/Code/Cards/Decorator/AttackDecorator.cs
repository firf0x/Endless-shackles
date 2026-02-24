using System.Collections.Generic;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards
{
    public class AttackDecorator : CardDecorator
    {
        public readonly int defaultDamageValue;
        public int currentDamageValue { get; private set; }
        private PlayerSystem player;
        private IDeck<CardData> defenceDeck;

        public AttackDecorator(int damageValue, PlayerSystem player, ICard<CardTypeEnum> card) : base(card)
        {
            this.defaultDamageValue = damageValue;
            ChangeDamage(0); // установка для того чтобы defaultDamageValue применился
            
            this.player = player;
            // this.defenceDeck = deck;
        }

        public override void Use(GameObject target)
        {
            base.Use(target);

            CardData data = target.GetComponent<CardData>();

            if(data.TryGetCardFeature<HealthDecorator>(out var feature) && !data.decorateCard.IgnoreLayers.HasFlag(IgnoreLayers))
            {
                //! Удаление карты атаки при нанесении урона по карте монстра
                if(data.decorateCard.Type.HasFlag(CardTypeEnum.Monster))
                {
                    Parent.GetComponent<CardData>().CardDestroy(false);
                }

                feature.TakeDamage(currentDamageValue);
            }
        }

        private void OnMonsterAttack()
        {
            if(defenceDeck.GetCardCount() > 0 )
            {
                foreach (var card in defenceDeck.cardDatas)
                {
                    if(card == null || card.decorateCard == null) continue;
                    if(card.TryGetCardFeature<HealthDecorator>(out var decorator))
                    {
                        decorator.TakeDamage(Parent.GetComponent<CardData>().GetCardFeature<AttackDecorator>().currentDamageValue);
                        break;
                    }
                }
            }
            else player.Kill();
        }

        public void ChangeDamage(int value)
        {
            currentDamageValue = defaultDamageValue + value;
            currentDamageValue = Mathf.Abs(currentDamageValue);
        }

        public override string ToString()
        {
            string message = $"Attack: было нанесено {currentDamageValue} урона.";
            Debug.Log(message);
            return message;
        }

        public override void Dispose()
        {
            // if (Parent.GetComponent<CardData>().TryGetCardFeature<StepCombatDecorator>(out var decorator)) decorator.OnStepInteraction -= OnMonsterAttack;
        }
    }
}