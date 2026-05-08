// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// namespace Game.Utils
// {
//     public sealed class InteractionHandler : MonoBehaviour
//     {
//         [SerializeReference, SubclassSelector] private List<CardAnimation> cardAnimation;

//         public void Start()
//         {
//             foreach (var anim in cardAnimation)
//             {
//                 anim?.Init();
//             }
//         }

//         public void InteractionPressed(InputAction.CallbackContext context)
//         {
//             foreach (var anim in cardAnimation)
//             {
//                 anim?.InteractionPressed(context);
//             }
//         }

//         public void InteractionReleased(InputAction.CallbackContext context)
//         {
//             foreach (var anim in cardAnimation)
//             {
//                 anim?.InteractionReleased(context);
//             }
//         }

//         public void isTarget(bool target)
//         {
//             foreach (var anim in cardAnimation)
//             {
//                 anim?.isTarget(target);
//             }
//         }

//         public void OnEnter()
//         {
//             foreach (var anim in cardAnimation)
//             {
//                 anim?.OnEnter();
//             }
//         }

//         public void Update()
//         {
//             foreach (var anim in cardAnimation)
//             {
//                 anim?.OnUpdate();
//             }
//         }

//         public void OnExit()
//         {
//             foreach (var anim in cardAnimation)
//             {
//                 anim?.OnExit();
//             }
//         }

//         public void OnDestroy()
//         {
//             foreach (var anim in cardAnimation)
//             {
//                 anim?.OnDestroy();
//             }            
//         }

//     }
// }