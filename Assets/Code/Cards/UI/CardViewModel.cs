using System;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.UI
{
    public class CardViewModel : IDisposable
    {
        private CardData model;

        public string Health { get; private set; }
        public string Damage { get; private set; }
        public string Step { get; private set; }

        public event Action UpdateUI;

        public CardViewModel(CardData model)
        {
            this.model = model;

            if(isAttackDecorator)
            {
                var m = model.GetCardFeature<AttackDecorator>();
                m.currentDamage.OnChanged += OnModelDamageChanged;
                m.currentDamage.Value = m.currentDamage.Value;
            }
            if(isHealthDecorator)
            {
                var m = model.GetCardFeature<HealthDecorator>();
                m.healthSystem.HealPoints.OnChanged += OnModelHPChanged;
                m.healthSystem.HealPoints.Value = m.healthSystem.HealPoints.Value;
            }
            if(isStepDecorator)
            {
                var m = model.GetCardFeature<StepCombatDecorator>();
                m.currentStep.OnChanged += OnModelStepChanged;
                m.currentStep.Value = m.currentStep.Value;
            }
        }

        public string Name => model.decorateCard.CardName;
        private void OnModelHPChanged(int value)
        {
            Health = value.ToString();
            UpdateUI?.Invoke();
        }

        private void OnModelDamageChanged(int value)
        {
            Damage = value.ToString();
            UpdateUI?.Invoke();
        }

        private void OnModelStepChanged(int value)
        {
            Step = value.ToString();
            UpdateUI?.Invoke();
        }

        public bool isAttackDecorator => model.CheckCardFeature<AttackDecorator>();
        public bool isHealthDecorator => model.CheckCardFeature<HealthDecorator>();
        public bool isStepDecorator => model.CheckCardFeature<StepCombatDecorator>();

        public void Dispose()
        {
            if(isAttackDecorator) model.GetCardFeature<AttackDecorator>().currentDamage.OnChanged -= OnModelDamageChanged;
            if(isHealthDecorator) model.GetCardFeature<HealthDecorator>().healthSystem.HealPoints.OnChanged -= OnModelHPChanged;
            if(isStepDecorator) model.GetCardFeature<StepCombatDecorator>().currentStep.OnChanged -= OnModelStepChanged;
        }
    }
}