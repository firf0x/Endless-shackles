// using System;
// using UnityEngine;
// using UnityEngine.InputSystem;

// namespace Game.Utils
// {
//     [Serializable]
//     public abstract class CardAnimation : ICardAnimation
//     {
//         protected bool target;

//         public virtual void Init() { }

//         public virtual void InteractionPressed(InputAction.CallbackContext context) { }
//         public virtual void InteractionReleased(InputAction.CallbackContext context) { }

//         public virtual void OnEnter() { }
//         public virtual void OnExit() { }

//         public virtual void OnUpdate() { }
//         public virtual void OnDestroy() { }

//         public void isTarget(bool target) => this.target = target;
//     }
// }