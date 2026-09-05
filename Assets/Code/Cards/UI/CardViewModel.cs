using System;
using System.Collections.Generic;
using System.Linq;
using Game.Cards.Modifier;
using Game.GameSystem;
using Game.Lib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    public class CardViewModel : IDisposable
    {
        private CardData model;

        private bool isHovered;
        public bool IsHovered => isHovered;

        // Text info
        public string Health { get; private set; }
        public string Damage { get; private set; }
        public string Step { get; private set; }

        // Modifiers events
        public IReadOnlyList<ModifierData> Modifiers { get; private set; }
        public event Action<ModifierData> ModifierAdded;
        public event Action<ModifierData> ModifierRemoved;

        public event Action UpdateUI;

        private CardAnimationComponents AnimationComponents = new CardAnimationComponents();

        public CardViewModel(CardData model, CardView cardView, CardAnimationConfig animationConfig, InputActionAsset inputAction)
        {
            this.model = model;

            AnimationComponents.Parent = model.gameObject;
            AnimationComponents.RenderObject = model.gameObject.transform.GetChild(0).gameObject;
            AnimationComponents.CardInfo = model;
            AnimationComponents.View = cardView;
            AnimationComponents.InputAction = inputAction;
            
            if(model.decorateCard.Type == CardTypeEnum.Monster) AnimationComponents.StateMachine = new MonsterCardStateMachine(AnimationComponents, animationConfig);
            else AnimationComponents.StateMachine = new HandCardStateMachine(AnimationComponents, animationConfig);

            AnimationComponents.StateMachine.ProcessEvent( CardAnimationStateEnum.Return );

            AnimationComponents.StateMachine.OnChangeState += OnStateChanged;   


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
            if(isModifierDecorator)
            {
                var m = model.GetCardFeature<ModifierDecorator>();
                m.OnAdded += OnModifierAdded;
                m.OnRemoved += OnModifierRemoved;

                Modifiers = m.Modifiers.ToList().AsReadOnly();
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

        #region Modifiers
        
        private void OnModifierAdded(ModifierData modifier)
        {
            var list = Modifiers.ToList();
            list.Add(modifier);
            Modifiers = list.AsReadOnly();
            
            ModifierAdded?.Invoke(modifier);
            UpdateUI?.Invoke();
        }

        private void OnModifierRemoved(ModifierData modifier)
        {
            var list = Modifiers.ToList();
            list.Remove(modifier);
            Modifiers = list.AsReadOnly();
            
            ModifierRemoved?.Invoke(modifier);
            UpdateUI?.Invoke();
        }

        private void OnModelModifierChanged(ModifierData modifierData)
        {

            // ModifierChanged?.Invoke();
            UpdateUI?.Invoke();
        }

        #endregion

        private void OnModelStepChanged(int value)
        {
            Step = value.ToString();
            UpdateUI?.Invoke();
        }

        #region StateMachine

        public void StateMachineUpdate() => AnimationComponents.StateMachine?.Update();

        public void OnHoverEnter()
        {
            isHovered = true;
            AnimationComponents.StateMachine?.OnHoverEnter();
        }

        public void OnHoverExit()
        {
            isHovered = false;
            AnimationComponents.StateMachine?.OnHoverExit();
        }

        public void OnInteractPerformed(InputAction.CallbackContext context) => AnimationComponents.StateMachine?.OnInteractPerformed(context);
        public void OnInteractCanceled(InputAction.CallbackContext context) => AnimationComponents.StateMachine?.OnInteractCanceled(context);

        private void OnStateChanged<State>(State oldState, State newState) where State : GenericHoverState
        {
            if (isHovered && oldState != null) newState?.OnHoverEnter();
        }

        #endregion

        public bool isAttackDecorator => model.CheckCardFeature<AttackDecorator>();
        public bool isHealthDecorator => model.CheckCardFeature<HealthDecorator>();
        public bool isModifierDecorator => model.CheckCardFeature<ModifierDecorator>();
        public bool isStepDecorator => model.CheckCardFeature<StepCombatDecorator>();

        public void Dispose()
        {
            if(isAttackDecorator) model.GetCardFeature<AttackDecorator>().currentDamage.OnChanged -= OnModelDamageChanged;
            if(isHealthDecorator) model.GetCardFeature<HealthDecorator>().healthSystem.HealPoints.OnChanged -= OnModelHPChanged;
            
            if (isModifierDecorator)
            {
                var m = model.GetCardFeature<ModifierDecorator>();
                m.OnAdded -= OnModifierAdded;
                m.OnRemoved -= OnModifierRemoved;
            }

            if(isStepDecorator) model.GetCardFeature<StepCombatDecorator>().currentStep.OnChanged -= OnModelStepChanged;

            AnimationComponents.StateMachine.OnChangeState -= OnStateChanged;
            AnimationComponents.StateMachine.OnDestroy();
            AnimationComponents.StateMachine = null;
        }
    }
}