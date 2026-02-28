using UnityEngine;
using Game.Lib;
using System;
using Game.GameSystem;

namespace Game.Cards
{
    public class StepCombatDecorator : CardDecorator, IStepTick
    {
        public ReactiveProperty<int> currentStep { get; private set; } = new();
        public event Action OnStepInteraction;
        private readonly int maxStep;
        private bool isSpawn = true;

        public StepCombatDecorator(int step, ICard<CardTypeEnum> card) : base(card)
        {
            maxStep = step;
            currentStep.Value = step;
            StepCombatSystem.Instance.EventUpdate += OnUpdate;
        }

        public void OnUpdate()
        {
            if(isSpawn)
            {
                isSpawn = false;
                return;
            }
            
            currentStep.Value--;

            if(currentStep.Value <= 0)
            {
                OnStepInteraction?.Invoke();

                currentStep.Value = maxStep;
            }
        }

        public override void Dispose()
        {
            OnStepInteraction = null;
            StepCombatSystem.Instance.EventUpdate -= OnUpdate;
            
            base.Dispose();
        }
    }
}