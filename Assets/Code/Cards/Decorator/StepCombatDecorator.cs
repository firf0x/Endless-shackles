using UnityEngine;
using Game.Lib;
using System;
using Game.GameSystem;

namespace Game.Cards
{
    public class StepCombatDecorator : CardDecorator, IStepTick
    {
        public int currentStep { get; private set; }
        public event Action OnStepInteraction;
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
            
            currentStep--;

            if(currentStep <= 0)
            {
                OnStepInteraction?.Invoke();

                currentStep = maxStep;
            }
        }

        public override void Dispose()
        {
            OnStepInteraction = null;
            StepCombatSystem.Instance.EventUpdate -= OnUpdate;
        }
    }
}