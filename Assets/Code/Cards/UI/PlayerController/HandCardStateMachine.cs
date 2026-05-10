using Game.Cards.UI.State;
using Game.Lib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    public class HandCardStateMachine : InteractionStateMachine<HandCardState, HandCardStateEnum>
    {
        private CardAnimationComponents<HandCardState, HandCardStateEnum> components;
        private CardAnimationConfig config;

        public HandCardStateMachine(CardAnimationComponents<HandCardState, HandCardStateEnum> components, CardAnimationConfig config)
        {
            this.components = components;
            this.config = config;

            AddState(HandCardStateEnum.Idle, new HandCardIdle(components, config));
            AddState(HandCardStateEnum.Hover, new HandCardHover(components, config));
            AddState(HandCardStateEnum.Drag, new HandCardDrag(components, config));
            AddState(HandCardStateEnum.Return, new HandCardReturn(components, config));
            
            ChangeState(States[HandCardStateEnum.Return]);
        }

        public override void ProcessEvent(HandCardStateEnum stateType)
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
    }
}