using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class MonsterCardHover : MonsterCardState
    {
        public MonsterCardHover(CardAnimationComponents<MonsterCardState, MonsterCardStateEnum> components, CardAnimationConfig config) : base(components, config) { }

        public override void OnHoverExit()
        {
            components.StateMachine.ProcessEvent(MonsterCardStateEnum.Idle);
        }
    }
}