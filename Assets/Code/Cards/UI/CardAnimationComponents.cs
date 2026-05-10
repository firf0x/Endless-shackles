using System;
using Game.Lib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    [Serializable]
    public sealed class CardAnimationComponents<TState, TEnum> where TState : GenericState where TEnum : Enum
    {
        public GameObject Parent;
        public GameObject RenderObject;
        public CardData CardInfo;
        public CardView View;
        public InteractionStateMachine<TState, TEnum> StateMachine;
        public InputActionAsset InputAction;
    }
}