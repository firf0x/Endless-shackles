using DG.Tweening;
using UnityEngine;

namespace Game.Cards.UI.State
{
    public class MonsterCardReturn : AnimationCardState
    {
        private Tween _moveTween;
        private Tween _rotateTween;

        public MonsterCardReturn(CardAnimationComponents components, CardAnimationConfig config) : base(components, config) { }

        public override void OnEnter()
        {
            _moveTween = components.Parent.transform
                .DOLocalMove(components.CardInfo.DeckPosition.Value, config.ReturnAnimationDuration)
                .SetEase(config.ReturnEase)
                .OnComplete(() =>
                {
                    components.StateMachine.ProcessEvent(CardAnimationStateEnum.Idle);
                });
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