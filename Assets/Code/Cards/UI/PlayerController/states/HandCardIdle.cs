using DG.Tweening;
using UnityEngine;

namespace Game.Cards.UI.State
{
    public class HandCardIdle : HandCardState
    {
        private Transform renderTransform;
        private Vector3 startLocalPosition;
        private Quaternion startLocalRotation;
        private Tween currentVerticalTween;
        private Tween currentRotationTween;

        public HandCardIdle(CardAnimationComponents<HandCardState, HandCardStateEnum> components, CardAnimationConfig config) : base(components, config) { }

        public override void OnEnter()
        {
            renderTransform = components.RenderObject.transform;
            startLocalPosition = renderTransform.localPosition;
            startLocalRotation = renderTransform.localRotation;

            StartVerticalMovement();
            StartRotationMovement();
        }

        public override void OnHoverEnter()
        {
            KillCurrentTweens();

            // renderTransform.DOLocalMove(startLocalPosition, 0.2f).SetEase(Ease.OutQuad);
            // renderTransform.DOLocalRotateQuaternion(startLocalRotation, 0.2f).SetEase(Ease.OutQuad);

            components.StateMachine.ProcessEvent(HandCardStateEnum.Hover);
        }

        public override void OnExit()
        {
            KillCurrentTweens();

            renderTransform.localPosition = startLocalPosition;
            renderTransform.localRotation = startLocalRotation;
        }

        private void StartVerticalMovement()
        {
            float targetY = Random.value > 0.5f 
                ? startLocalPosition.y + config.IdleVerticalAmplitude 
                : startLocalPosition.y - config.IdleVerticalAmplitude;
            
            float duration = Random.Range(config.IdleVerticalMinDuration, config.IdleVerticalMaxDuration);

            currentVerticalTween = renderTransform
                .DOLocalMoveY(targetY, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => StartVerticalMovementOpposite(targetY));
        }

        private void StartVerticalMovementOpposite(float currentY)
        {
            // Возвращаемся к другой крайней точке
            float targetY = Mathf.Approximately(currentY, startLocalPosition.y + config.IdleVerticalAmplitude)
                ? startLocalPosition.y - config.IdleVerticalAmplitude
                : startLocalPosition.y + config.IdleVerticalAmplitude;
            
            float duration = Random.Range(config.IdleVerticalMinDuration, config.IdleVerticalMaxDuration);

            currentVerticalTween = renderTransform
                .DOLocalMoveY(targetY, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => StartVerticalMovementOpposite(targetY));
        }

        private void StartRotationMovement()
        {
            // Случайно начинаем поворот влево (отрицательный) или вправо (положительный)
            float targetZ = Random.value > 0.5f 
                ? startLocalRotation.eulerAngles.z + config.IdleRotationAngle 
                : startLocalRotation.eulerAngles.z - config.IdleRotationAngle;
            
            float duration = Random.Range(config.IdleRotationMinDuration, config.IdleRotationMaxDuration);

            currentRotationTween = renderTransform
                .DOLocalRotate(new Vector3(0, 0, targetZ), duration, RotateMode.Fast)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => StartRotationMovementOpposite(targetZ));
        }

        private void StartRotationMovementOpposite(float currentZ)
        {
            float targetZ = startLocalRotation.eulerAngles.z + 
                (Mathf.Approximately(currentZ, startLocalRotation.eulerAngles.z + config.IdleRotationAngle) 
                    ? -config.IdleRotationAngle 
                    : config.IdleRotationAngle);
            
            float duration = Random.Range(config.IdleRotationMinDuration, config.IdleRotationMaxDuration);

            currentRotationTween = renderTransform
                .DOLocalRotate(new Vector3(0, 0, targetZ), duration, RotateMode.Fast)
                .SetEase(Ease.InOutSine)
                .OnComplete(() => StartRotationMovementOpposite(targetZ));
        }

        private void KillCurrentTweens()
        {
            currentVerticalTween?.Kill();
            currentRotationTween?.Kill();
            currentVerticalTween = null;
            currentRotationTween = null;
        }
    }
}