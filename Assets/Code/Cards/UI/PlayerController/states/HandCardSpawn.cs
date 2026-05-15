using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI.State
{
    public class HandCardSpawn : HandCardState
    {
        private GameObject backFace;
        private GameObject forwardFace;

        public HandCardSpawn(CardAnimationComponents<HandCardState, HandCardStateEnum> components, CardAnimationConfig config) : base(components, config) { }

        public override void OnEnter()
        {
            components.Parent.transform.GetChild(1);
        }
    }
}