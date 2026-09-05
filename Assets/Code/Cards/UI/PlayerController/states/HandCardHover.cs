using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class HandCardHover : AnimationCardState
    {
        // private InputAction positionAction;

        public HandCardHover(CardAnimationComponents components, CardAnimationConfig config) : base(components, config) { }

        public override void OnClick(InputAction.CallbackContext context)
        {
            if(components.CardInfo.decorateCard.isDrag) components.StateMachine.ProcessEvent(CardAnimationStateEnum.Drag);
        }

        public override void OnHoverExit()
        {
            components.StateMachine.ProcessEvent(CardAnimationStateEnum.Idle);
        }
    }
}