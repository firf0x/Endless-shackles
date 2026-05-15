using DG.Tweening;
using UnityEngine;

namespace Game.Cards.UI.State
{
    public class HandCardReturn : HandCardState
    {
        private Tween _moveTween;
        private Tween _rotateTween;

        public HandCardReturn(CardAnimationComponents<HandCardState, HandCardStateEnum> components, CardAnimationConfig config) : base(components, config) { }

        public override void OnEnter()
        {
            _moveTween = components.Parent.transform
                .DOLocalMove(components.CardInfo.DeckPosition.Value, config.ReturnAnimationDuration)
                .SetEase(config.ReturnEase)
                .OnComplete(() =>
                {
                    components.StateMachine.ProcessEvent(HandCardStateEnum.Idle);
                });

            _rotateTween = components.RenderObject.transform
                .DOLocalRotate(Vector3.zero, config.ReturnAnimationDuration, RotateMode.Fast);

        }

        public override void OnExit()
        {
            KillCurrentTweens();
        }

        private void KillCurrentTweens()
        {
            _moveTween?.Kill();
            _rotateTween?.Kill();
            _moveTween = null;
            _rotateTween = null;
        }
    }
}