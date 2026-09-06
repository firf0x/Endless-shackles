using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class HandCardDrag : AnimationCardState
    {
        private InputAction positionAction;
        private Vector3 velocityRef;
        private Vector2 lastMousePos;
        
        public HandCardDrag(CardAnimationComponents components, CardAnimationConfig config) : base(components, config) { }

        public override void OnEnter()
        {
            var playerActionMap = components.InputAction.FindActionMap("Player");
            positionAction = playerActionMap?.FindAction("Mouse_position");

            components.RenderObject.transform.DOScale(config.DragScale, config.DragScaleAnimationDuration);
        }

        public override void OnUpdate()
        {
            Vector3 mouseScreenPos = GetMouseWorldPosition();

            components.Parent.transform.position = Vector3.SmoothDamp(components.Parent.transform.position, mouseScreenPos, ref velocityRef, config.DragFollowSmoothTime);
        
            float targetZRotation = Mathf.Clamp(-velocityRef.x * config.DragRotationFactor, -config.DragRotationAngle, config.DragRotationAngle);
            components.RenderObject.transform.eulerAngles = new Vector3(components.RenderObject.transform.position.x, components.RenderObject.transform.position.y, targetZRotation);
            
            lastMousePos = mouseScreenPos;
        }

        public override void OnRealise(InputAction.CallbackContext context)
        {
            components.StateMachine?.ProcessEvent(CardAnimationStateEnum.Return);
        }

        public override void OnExit()
        {
            // components.Parent.GetComponent<BoxCollider2D>().enabled = true;
            components.RenderObject.transform.DOScale(1, config.DragScaleAnimationDuration);
            positionAction = null;
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
            Vector3 vector = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, -Camera.main.transform.position.z));
            return vector;
        }
    }
}