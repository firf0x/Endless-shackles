using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class MonsterCardHover : AnimationCardState
    {
        public MonsterCardHover(CardAnimationComponents components, CardAnimationConfig config) : base(components, config) { }

        public override void OnHoverExit()
        {
            components.StateMachine.ProcessEvent(CardAnimationStateEnum.Idle);
        }
    }
}