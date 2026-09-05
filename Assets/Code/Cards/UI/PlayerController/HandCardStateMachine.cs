using Game.Cards.UI.State;
using Game.Lib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    public class HandCardStateMachine : InteractionStateMachine<AnimationCardState, CardAnimationStateEnum>
    {
        private CardAnimationComponents components;
        private CardAnimationConfig config;

        public HandCardStateMachine(CardAnimationComponents components, CardAnimationConfig config)
        {
            this.components = components;
            this.config = config;

            components.CardInfo.decorateCard.OnDestroy += OnDead;

            AddState(CardAnimationStateEnum.Idle, new HandCardIdle(components, config));
            AddState(CardAnimationStateEnum.Hover, new HandCardHover(components, config));
            AddState(CardAnimationStateEnum.Drag, new HandCardDrag(components, config));
            // State Hit
            // State Damage
            AddState(CardAnimationStateEnum.Return, new HandCardReturn(components, config));
            AddState(CardAnimationStateEnum.Dead, new HandCardDead(components, config));
            
            ChangeState(States[CardAnimationStateEnum.Return]);
        }

        public override void ProcessEvent(CardAnimationStateEnum stateType)
        {
            if( States.TryGetValue(stateType, out var state) )
            {
                ChangeState(state);
            }
        }

        public override void Update()
        {
            currentState?.OnUpdate();
        }

        public override void OnDestroy()
        {
            currentState?.OnExit();
        }

        public override void OnHoverEnter()
        {
            currentState?.OnHoverEnter();
        }

        public override void OnHoverExit()
        {
            currentState?.OnHoverExit();
        }

        public override void OnInteractPerformed(InputAction.CallbackContext context)
        {
            currentState?.OnClick(context);
        }

        public override void OnInteractCanceled(InputAction.CallbackContext context)
        {
            currentState?.OnRealise(context);
        }

        private void OnDead()
        {
            ProcessEvent(CardAnimationStateEnum.Dead);
            components.CardInfo.decorateCard.OnDestroy -= OnDead;
        }
    }
}