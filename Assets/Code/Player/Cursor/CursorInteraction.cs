using Game.Cards;
using Game.Deck;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Utils
{
    public class CursorInteraction : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputAction;
        [SerializeField] private LayerMask interactableLayer;

        private InputAction interactAction;
        private InputAction positionAction;

        // private InteractionHandler currentHandler;
        // private InteractionHandler activeHandler;

        private CardData currentCard;


        private void Awake()
        {
            var playerActionMap = inputAction.FindActionMap("Player");

            interactAction = playerActionMap.FindAction("Interact");
            positionAction = playerActionMap.FindAction("Mouse_position");

            // interactAction.performed += OnInteractPerformed;
            // interactAction.canceled += OnInteractCanceled;
        }

        private void OnEnable()
        {
            interactAction.Enable();
            positionAction.Enable();
        }

        private void OnDisable()
        {
            interactAction.Disable();
            positionAction.Disable();
        }

        private void OnDestroy()
        {
            // interactAction.performed -= OnInteractPerformed;
            // interactAction.canceled -= OnInteractCanceled;
        }

        // private void Update()
        // {
        //     CheckHover();
        // }

        // private void CheckHover()
        // {
        //     Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
        //     Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        //     RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero, 25f, interactableLayer);
            
        //     InteractionHandler newHandler = null;

        //     foreach (var hit in hits)
        //     {
        //         var handler = hit.collider.GetComponent<InteractionHandler>();
        //         if (handler != null)
        //         {
        //             newHandler = handler;
        //             break;
        //         }
        //     }

        //     if (currentHandler != newHandler)
        //     {
        //         if (currentHandler != null)
        //         {
        //             currentHandler.OnExit();
        //             currentHandler.isTarget(false);
        //         }
        //         currentHandler = newHandler;
        //         if (currentHandler != null)
        //         {
        //             currentHandler.isTarget(true);
        //             currentHandler.OnEnter();
        //         }
        //     }
        // }

        // private void OnInteractPerformed(InputAction.CallbackContext context)
        // {
        //     Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
        //     Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        //     RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 10f, interactableLayer);

        //     if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & interactableLayer) != 0)
        //     {
        //         CardData card;
                
        //         if (hit.collider.TryGetComponent<CardData>(out card) && !hit.collider.GetComponent<CardData>().decorateCard.isLocked)
        //         {
        //             if(card.decorateCard.Type == CardTypeEnum.Monster) return;
                    
        //             currentCard = card;
        //         }
        //     }

        //     if (currentHandler != null)
        //     {       
        //         activeHandler = currentHandler;
        //         activeHandler.InteractionPressed(context);
        //     }
        // }

        // private void OnInteractCanceled(InputAction.CallbackContext context)
        // {
        //     Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
        //     Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        //     RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero, 25f, interactableLayer);

        //     if (currentCard != null)
        //     {
        //         foreach (RaycastHit2D hit in hits)
        //         {
        //             if (hit.collider != null && hit.collider.GetComponent<CardData>() != null && hit.collider.GetComponent<CardData>() != currentCard)
        //             {
        //                 currentCard.Execute(hit.collider.gameObject);
        //                 break;
        //             }

        //             if (hit.collider != null && hit.collider.gameObject.layer == 8)
        //             {
        //                 string zoneTypeName = hit.collider.gameObject.name;

        //                 if (currentCard.TryGetCardFeature<CustomTypeDecorator<DefenceType>>(out var decorator))
        //                 {
        //                     if (zoneTypeName == decorator.CustomType.ToString())
        //                     {
        //                         DefendDeck.Instance.AddCard(currentCard);
        //                         HandDeck.Instance.RemoveCard(currentCard, false);
        //                         break;
        //                     }
        //                 }
        //             }
        //         }
        //     }

        //     if(activeHandler != null)
        //     {
        //         activeHandler.InteractionReleased(context);
        //         activeHandler = null;
        //     }
        // }
    }
}