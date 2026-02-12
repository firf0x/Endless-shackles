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

        public StepCombatDecorator(int step, PlayerSystem player, ICard<CardTypeEnum> card) : base(card)
        {
            maxStep = step;
            currentStep = step;
            this.player = player;
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
            
            if(currentStep <= 0)
            {
                

                player.Kill();
                currentStep = maxStep;
            }
        }

        public override void Dispose()
        {
            StepCombatSystem.Instance.EventUpdate -= OnUpdate;
        }
    }
}