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
        public int MaxStep { get; private set; }
        private bool isSpawn = true;

        public StepCombatDecorator(int step, ICard<CardTypeEnum> card) : base(card)
        {
            MaxStep = step;
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

                currentStep.Value = MaxStep;
            }
        }

        public void ChangeLimits(int value)
        {
            value = Mathf.Max(value, 1);

            MaxStep = value;
        }

        public override void Dispose()
        {
            OnStepInteraction = null;
            StepCombatSystem.Instance.EventUpdate -= OnUpdate;

            base.Dispose();
        }
    }
}