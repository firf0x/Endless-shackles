using System;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;

namespace Game.Cards.UI
{
    public class CardViewModel
    {
        private CardData model;

        public CardViewModel(CardData model)
        {
            this.model = model;
        }

        public string Name => model.decorateCard.CardName;
        public string Damage => model.GetCardFeature<AttackDecorator>().currentDamageValue.ToString();
        public string Health => model.GetCardFeature<HealthDecorator>().healthSystem.HealPoints.Value.ToString();
        public string Step => model.GetCardFeature<StepCombatDecorator>().currentStep.ToString();

        public bool isAttackDecorator => model.CheckCardFeature<AttackDecorator>();
        public bool isHealthDecorator => model.CheckCardFeature<HealthDecorator>();
        public bool isStepDecorator => model.CheckCardFeature<StepCombatDecorator>();
    }
}