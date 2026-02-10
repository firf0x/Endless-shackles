using UnityEngine;
using Game.Lib;
using System;

namespace Game.Cards
{
    public class StepCombatDecorator : CardDecorator, IStepTick
    {
        public int currentStep { get; private set; }
        private readonly int maxStep;

        public StepCombatDecorator(int step, ICard<CardTypeEnum> card) : base(card)
        {
            maxStep = step;
            currentStep = step;
        }

        public void OnUpdate()
        {
            if(currentStep <= 0)
            {
                
            }
        }
    }
}