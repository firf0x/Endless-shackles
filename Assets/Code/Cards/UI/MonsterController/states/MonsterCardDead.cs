using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class MonsterCardDead : AnimationCardState
    {
        private SpriteRenderer BoardRender;
        private SpriteRenderer imageRender;
        private SpriteRenderer OtherRender;
        private MaterialPropertyBlock propertyBlock;
        private float burnProgress = 0f;
        private float animationDuration = 1.0f;
        public MonsterCardDead(CardAnimationComponents components, CardAnimationConfig config) : base(components, config)
        {
            OtherRender = components.RenderObject.transform.GetComponent<SpriteRenderer>();
        
            BoardRender = components.RenderObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            imageRender = components.RenderObject.transform.GetChild(1).GetComponent<SpriteRenderer>();

            propertyBlock = new MaterialPropertyBlock();
            OtherRender.GetPropertyBlock(propertyBlock);

            burnProgress = 0f;
            propertyBlock.SetFloat("_BurnProgress", burnProgress);

            OtherRender.SetPropertyBlock(propertyBlock);
            imageRender.SetPropertyBlock(propertyBlock);
            BoardRender.SetPropertyBlock(propertyBlock);
        }

        public override void OnEnter()
        {
            components.CardInfo.StartCoroutine(AnimateBurning());
        }

        public override void OnExit()
        {
            OtherRender.SetPropertyBlock(null);
            imageRender.SetPropertyBlock(null);
            BoardRender.SetPropertyBlock(null);

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
                
                OtherRender.SetPropertyBlock(propertyBlock);
                imageRender.SetPropertyBlock(propertyBlock);
                BoardRender.SetPropertyBlock(propertyBlock);

                yield return null;
            }

            components.CardInfo.CardDestroy(true);
        }
    }
}