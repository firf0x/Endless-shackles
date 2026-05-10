using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class HandCardDrag : HandCardState
    {
        private InputAction positionAction;
        private Vector3 velocityRef;

        public HandCardDrag(CardAnimationComponents<HandCardState, HandCardStateEnum> components, CardAnimationConfig config) : base(components, config) { }

        public override void OnEnter()
        {
            var playerActionMap = components.InputAction.FindActionMap("Player");
            positionAction = playerActionMap?.FindAction("Mouse_position");
            // positionAction?.Enable();
        }

        public override void OnUpdate()
        {
            Vector3 mouseScreenPos = GetMouseWorldPosition();

            components.Parent.transform.position = Vector3.SmoothDamp(components.Parent.transform.position, mouseScreenPos, ref velocityRef, config.FollowSmoothTime);
        }

        public override void OnRealise(InputAction.CallbackContext context)
        {
            components.StateMachine?.ProcessEvent(HandCardStateEnum.Return);
        }

        public override void OnExit()
        {
            // positionAction?.Disable();
            positionAction = null;
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
            return Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -Camera.main.transform.position.z));
        }
    }
}