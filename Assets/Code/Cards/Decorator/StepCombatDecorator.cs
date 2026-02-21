using UnityEngine;
using Game.Lib;
using System;
using Game.GameSystem;

namespace Game.Cards
{
    public class StepCombatDecorator : CardDecorator, IStepTick
    {
        public int currentStep { get; private set; }
        private readonly int maxStep;
        private bool isSpawn = true;
        private PlayerSystem player;
        private IDeck<CardData> defendDeck;

        public StepCombatDecorator(int step, PlayerSystem player, IDeck<CardData> deck, ICard<CardTypeEnum> card) : base(card)
        {
            maxStep = step;
            currentStep = step;
            this.player = player;
            defendDeck = deck;
            StepCombatSystem.Instance.EventUpdate += OnUpdate;
        }

        public void OnUpdate()
        {
            if(isSpawn)
            {
                isSpawn = false;
                return;
            }
            else currentStep--;
            
            // foreach (var item in defendDeck.cardDatas)
            // {
            //     Debug.Log(item);
            // }

            if(currentStep <= 0)
            {
                if(defendDeck.GetCardCount() > 0 )
                {
                    foreach (var card in defendDeck.cardDatas)
                    {
                        if(card == null || card.decorateCard == null) continue;
                        // Debug.Log(card.decorateCard.CardName);
                        if(card.TryGetCardFeature<HealthDecorator>(out var decorator))
                        {
                            decorator.TakeDamage(Parent.GetComponent<CardData>().GetCardFeature<AttackDecorator>().currentDamageValue);
                            break;
                        }
                    }
                }
                else player.Kill();

                defendDeck.UpdateAllCardsPosition();

                currentStep = maxStep;
            }
        }

        public override void Dispose()
        {
            StepCombatSystem.Instance.EventUpdate -= OnUpdate;
        }
    }
}