using System;
using Game.Lib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    [Serializable]
    public sealed class CardAnimationComponents
    {
        public GameObject Parent;
        public GameObject RenderObject;
        public CardData CardInfo;
        public CardView View;
        public InteractionStateMachine<AnimationCardState, CardAnimationStateEnum> StateMachine;
        public InputActionAsset InputAction;
    }
}