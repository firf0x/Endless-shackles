using UnityEngine;
using Game.Lib;
using System;
using Game.GameSystem;

namespace Game.Cards
{
    public class StepCombatDecorator : CardDecorator, IStepTick
    {
        public ReactiveProperty<int> currentStep { get; private set; } = new();
        public event Action OnStepStartUpdate;
        public event Action OnStepEndUpdate;
        public int MaxStep { get; private set; }
        private bool isSpawn = true;

        /// <summary>
        /// Показывает активен ли текущий декоратор
        /// </summary>
        public bool IsActive { get; private set; } = true;

        public StepCombatDecorator(int step, ICard card) : base(card)
        {
            MaxStep = step;
            currentStep.Value = step;
            StepCombatSystem.Instance.EventUpdate += OnUpdate;
        }

        public void OnUpdate()
        {
            // if(isSpawn)
            // {
            //     isSpawn = false;
            //     return;
            // }
            
            if(!IsActive || decoratedCard == null || isLocked) return;
            
            currentStep.Value--;

            if(currentStep.Value <= 0)
            {
                OnStepStartUpdate?.Invoke();

                currentStep.Value = MaxStep;

                OnStepEndUpdate?.Invoke();
            }
        }

        /// <summary>
        /// Метод позволяющий отключать счётчик ходов.
        /// </summary>
        /// <param name="isActive">Если задать true, то тогда будет false</param>
        public void Stop(bool isActive) => IsActive = !isActive;

        public void ChangeLimits(int value)
        {
            value = Mathf.Max(value, 1);

            MaxStep = value;
        }

        public override void Dispose()
        {
            OnStepStartUpdate = null;
            StepCombatSystem.Instance.EventUpdate -= OnUpdate;

            base.Dispose();
        }
    }
}