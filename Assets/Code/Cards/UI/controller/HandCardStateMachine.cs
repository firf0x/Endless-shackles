using Game.Lib;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    public class HandCardStateMachine : InteractionStateMachine<HandCardState, HandCardStateEnum>
    {
        private HandCardComponents components;
        private HandCardConfig config;
        private InputActionAsset input;

        private InputAction interactAction;
        private InputAction positionAction;

        public HandCardStateMachine(HandCardComponents components, HandCardConfig config, InputActionAsset input)
        {
            this.components = components;
            this.config = config;
            this.input = input;

            var playerActionMap = this.input.FindActionMap("Player");

            interactAction = playerActionMap.FindAction("Interact");
            positionAction = playerActionMap.FindAction("Mouse_position");

            // AddState();
        }

        public override void ProcessEvent(HandCardStateEnum stateType)
        {
            if( States.TryGetValue(stateType, out var state) )
            {
                interactAction.performed -= currentState.OnClick;
                interactAction.canceled -= currentState.OnRealise;

                ChangeState(state);

                interactAction.performed += currentState.OnClick;
                interactAction.canceled += currentState.OnRealise;
            }
        }

        public override void Update()
        {
            currentState?.OnUpdate();
        }

        public override void OnDestroy()
        {
            currentState?.OnClear();
        }

        public override void OnHoverEnter()
        {
            // наведение
        }

        public override void OnHoverExit()
        {
            // вывел
        }

        public override void OnInteractPerformed()
        {
            // нажал
        }

        public override void OnInteractCanceled()
        {
            // отпустил
        }
    }
}