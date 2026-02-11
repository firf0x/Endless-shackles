using UnityEngine;
using Game.Lib;
using System;
using Game.GameSystem;

namespace Game.Cards
{
    public class StepCombatDecorator : CardDecorator, IStepTick
    {
        public int currentStep { get; private set; }
        public event Action<int> OnStepChanged;
        
        private readonly int maxStep;

        private IDeck<CardData> enemyDeck;
        private IDamageble player;

        public StepCombatDecorator(int step, IDeck<CardData> deck, IDamageble player, ICard<CardTypeEnum> card) : base(card)
        {
            this.player = player;
            maxStep = step;
            currentStep = step;
            enemyDeck = deck;

            StepCombatSystem.Instance.EventUpdate += OnUpdate;
        }

        public void OnUpdate()
        {
            Debug.Log($"{CardName} | currentStep : {currentStep}");

            --currentStep;

            if(currentStep <= 0)
            {
                foreach (var item in enemyDeck.cardDatas)
                {
                    if(item != null)
                    {
                        Parent.GetComponent<CardData>().TryGetCardFeature<AttackDecorator>(out var attackData);
                        attackData.Use(item.gameObject);
                        break;
                    }
                }

                if(enemyDeck.GetCardCount() <= 0)
                {
                    // Parent.GetComponent<CardData>().TryGetCardFeature<AttackDecorator>(out var attackData);
                    // attackData.GetEffect<EffectIgnoreShields>
                    player.TakeDamage(100);
                }

                currentStep = maxStep;
            }
        }

        public override void Dispose()
        {
            StepCombatSystem.Instance.EventUpdate -= OnUpdate;
        }
    }
}