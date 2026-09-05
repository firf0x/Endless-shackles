using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class HandCardDead : HandCardState
    {
        private SpriteRenderer renderer;
        private MaterialPropertyBlock propertyBlock;
        private float burnProgress = 0f;
        private float animationDuration = 1.0f;
        public HandCardDead(CardAnimationComponents<HandCardState, HandCardStateEnum> components, CardAnimationConfig config) : base(components, config)
        {
            renderer = components.CardInfo.transform.GetChild(0).GetComponent<SpriteRenderer>();

            propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);

            burnProgress = 0f;
            propertyBlock.SetFloat("_BurnProgress", burnProgress);
            renderer.SetPropertyBlock(propertyBlock);
        }

        public override void OnEnter()
        {
            components.CardInfo.StartCoroutine(AnimateBurning());
        }

        public override void OnExit()
        {
            propertyBlock.Clear();
            propertyBlock = null;
        }

        private System.Collections.IEnumerator AnimateBurning()
        {
            float elapsed = 0f;
            while (elapsed < animationDuration)
            {
                elapsed += Time.deltaTime;
                burnProgress = Mathf.Clamp01(elapsed / animationDuration);
                
                propertyBlock.SetFloat("_BurnProgress", burnProgress);
                renderer.SetPropertyBlock(propertyBlock);
                
                yield return null;
            }

            components.CardInfo.CardDestroy(true);
        }
    }
}