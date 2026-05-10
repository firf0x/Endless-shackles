using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class HandCardHover : HandCardState
    {
        // private InputAction positionAction;

        public HandCardHover(CardAnimationComponents<HandCardState, HandCardStateEnum> components, CardAnimationConfig config) : base(components, config) { }

        public override void OnClick(InputAction.CallbackContext context)
        {
            if(components.CardInfo.decorateCard.isDrag) components.StateMachine.ProcessEvent(HandCardStateEnum.Drag);
        }

        public override void OnHoverExit()
        {
            components.StateMachine.ProcessEvent(HandCardStateEnum.Idle);
        }
    }
}