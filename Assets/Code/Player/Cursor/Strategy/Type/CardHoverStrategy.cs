// using System;
// using DG.Tweening;
// using Game.Cards;
// using UnityEngine;
// using UnityEngine.InputSystem;

// namespace Game.Utils
// {
//     [Serializable]
//     public class CardHoverStrategy : CardAnimation
//     {
//         [SerializeField] private CardData cardData;
//         [SerializeField] private float hoverHeight = 1f;
//         [SerializeField] private float animationDuration = 0.2f;
//         [SerializeField] private Ease type = Ease.OutQuad;

//         private Tween tween;
//         private Tween tweenReturn;
//         private bool isHover = true;

//         public override void Init()
//         {
//             // Подписываемся на изменение базовой позиции
//             cardData.DeckPosition.OnChanged += OnDeckPositionChanged;
//             CreateHoverTween();
//         }

//         private void OnDeckPositionChanged(Vector3 newDeckPos)
//         {
//             CreateHoverTween();
//         }

//         private void CreateHoverTween()
//         {
//             tween?.Kill();
//             Vector3 endPos = cardData.DeckPosition.Value + Vector3.up * hoverHeight;
//             tween = cardData.transform.DOMove(endPos, animationDuration)
//                 .SetEase(type)
//                 .SetAutoKill(false)
//                 .Pause();
//         }

//         public override void OnEnter()
//         {
//             if (tween != null && tween.IsActive() && isHover && cardData.decorateCard.isDrag)
//             {
//                 if (tweenReturn != null && tweenReturn.IsActive())
//                     tweenReturn.Kill();

//                 // Обязательно «перематываем» к новой стартовой позиции
//                 tween.Rewind();
//                 tween.PlayForward();
//             }
//         }

//         public override void OnExit()
//         {
//             if (tween != null && tween.IsActive() && isHover && cardData.decorateCard != null && cardData.decorateCard.isDrag)
//                 ReturnToDeckPosition();
//         }

//         public override void InteractionPressed(InputAction.CallbackContext context)
//         {
//             isHover = false;
//         }

//         public override void InteractionReleased(InputAction.CallbackContext context)
//         {
//             isHover = true;
//             if (tween != null && tween.IsActive() && cardData.decorateCard != null && cardData.decorateCard.isDrag)
//                 ReturnToDeckPosition();
//         }

//         private void ReturnToDeckPosition()
//         {
//             if (cardData == null) return;

//             if (tweenReturn != null && tweenReturn.IsActive())
//                 tweenReturn.Kill();

//             // Используем актуальную DeckPosition
//             tweenReturn = cardData.transform.DOMove(cardData.DeckPosition.Value, animationDuration)
//                 .SetEase(Ease.OutQuad)
//                 .OnComplete(() =>
//                 {
//                     tweenReturn?.Kill();
//                     tweenReturn = null;
//                 });
//         }

//         public override void OnDestroy()
//         {
//             if (cardData != null)
//                 cardData.DeckPosition.OnChanged -= OnDeckPositionChanged;
//             tween?.Kill();
//             tweenReturn?.Kill();
//         }
//     }
// }