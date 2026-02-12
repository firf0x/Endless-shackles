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
        private bool isSpawn = true;

        public StepCombatDecorator(int step, ICard<CardTypeEnum> card) : base(card)
        {
            maxStep = step;
            currentStep = step;

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
            
            // Debug.Log($"{CardName} | currentStep : {currentStep}");

            if(currentStep <= 0)
            {
                currentStep = maxStep;
            }

            OnStepChanged?.Invoke(currentStep);
        }

        public override void Dispose()
        {
            StepCombatSystem.Instance.EventUpdate -= OnUpdate;
        }
    }
}