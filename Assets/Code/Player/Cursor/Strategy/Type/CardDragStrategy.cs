// using System;
// using DG.Tweening;
// using Game.Cards;
// using Game.Utils;
// using UnityEngine;
// using UnityEngine.InputSystem;

// [Serializable]
// public class CardDragStrategy : CardAnimation
// {
//     [SerializeField] private InputActionAsset inputAction;
//     [SerializeField] private CardData cardData;
//     [SerializeField] private float animationDuration = 0.2f;
//     [SerializeField] private Ease type = Ease.OutQuad;

//     private InputAction positionAction;

//     private bool isDragging;
//     private Tween dragTween;

//     public override void OnEnter()
//     {
//         var playerActionMap = inputAction.FindActionMap("Player");
//         positionAction = playerActionMap.FindAction("Mouse_position");
//     }

//     public override void OnUpdate()
//     {
//         if(isDragging && cardData.decorateCard.isDrag)
//         {
//             dragTween?.Kill();
//             Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
//             Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, 1f));

//             dragTween = cardData.transform.DOMove(mouseWorldPos, animationDuration).SetEase(type);
//         }
//     }

//     public override void InteractionPressed(InputAction.CallbackContext context)
//     {
//         isDragging = true;
//     }

//     public override void InteractionReleased(InputAction.CallbackContext context)
//     {
//         isDragging = false;

//         dragTween?.Kill();
//     }

//     public override void OnDestroy()
//     {
//         positionAction = null;
//         dragTween?.Kill();
//         dragTween = null;
//     }
// }