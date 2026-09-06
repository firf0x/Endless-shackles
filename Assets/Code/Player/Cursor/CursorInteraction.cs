using Game.Cards;
using Game.Cards.UI;
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

        private CardData currentCard;
        private CardData hoveredCard;

        private InteractionHandler currentInteractionHandler;
        private InteractionHandler hoveredInteractionHandler;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;

            var playerActionMap = inputAction.FindActionMap("Player");
            interactAction = playerActionMap.FindAction("Interact");
            positionAction = playerActionMap.FindAction("Mouse_position");

            interactAction.performed += OnInteractPerformed;
            interactAction.canceled += OnInteractCanceled;
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
            interactAction.performed -= OnInteractPerformed;
            interactAction.canceled -= OnInteractCanceled;
        }

        private void Update()
        {
            Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.nearClipPlane));

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 50f, interactableLayer);
            GameObject hitObject = hit.collider != null ? hit.collider.gameObject : null;
            CardData newHoveredCard = hit.collider != null ? hit.collider.GetComponent<CardData>() : null;
            InteractionHandler newHoveredHandler = hitObject != null ? hitObject.GetComponent<InteractionHandler>() : null;

            if (newHoveredCard != hoveredCard)
            {
                if (hoveredCard != null)
                {
                    CardView prevStateMachine = GetViewFromCard(hoveredCard);

                    prevStateMachine?.OnHoverExit();
                }

                hoveredCard = newHoveredCard;
                if (hoveredCard != null)
                {
                    CardView newStateMachine = GetViewFromCard(hoveredCard);
                    newStateMachine?.OnHoverEnter();
                }
            }

            if (newHoveredHandler != hoveredInteractionHandler)
            {
                hoveredInteractionHandler = newHoveredHandler;
            }
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.nearClipPlane));

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 50f, interactableLayer);
            GameObject hitObject = hit.collider != null ? hit.collider.gameObject : null;

            CardData card = hitObject != null ? hitObject.GetComponent<CardData>() : null;
            if (card != null)
            {
                if(GetStateTypeFromCard(card) == CardAnimationStateEnum.Return) return;
                currentCard = card;
                CardView view = GetViewFromCard(currentCard);
                
                view?.OnInteractPerformed(context);
                return;
            }

            InteractionHandler handler = hitObject != null ? hitObject.GetComponent<InteractionHandler>() : null;
            if (handler != null)
            {
                currentInteractionHandler = handler;
                handler.InteractionPressed(context);
            }
        }

        private void OnInteractCanceled(InputAction.CallbackContext context)
        {
            if (currentCard != null)
            {
                Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCamera.transform.position.z));
                RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector3.forward, 25f, interactableLayer);

                foreach (RaycastHit2D hit in hits)
                {
                    Debug.Log(hit.collider.name);

                    if (hit.collider != null && hit.collider.gameObject.layer == 8)
                    {
                        string zoneTypeName = hit.collider.gameObject.name;

                        if (currentCard.TryGetCardFeature<CustomTypeDecorator<DefenceType>>(out var decorator))
                        {
                            if (zoneTypeName == decorator.CustomType.ToString())
                            {
                                DefendDeck.Instance.AddCard(currentCard);
                                currentCard.GetComponent<BoxCollider2D>().enabled = false;
                                currentCard.DeckPosition.Value = hit.collider.gameObject.transform.position;
                                HandDeck.Instance.RemoveCard(currentCard, false);
                                break;
                            }
                        }
                    }

                    if (hit.collider != null && hit.collider.GetComponent<CardData>() != null && hit.collider.GetComponent<CardData>() != currentCard)
                    {
                        currentCard.Execute(hit.collider.gameObject);
                        break;
                    }
                }

                CardView view = GetViewFromCard(currentCard);
                view?.OnInteractCanceled(context);

                currentCard = null;
                return;
            }

            if (currentInteractionHandler != null)
            {
                currentInteractionHandler.InteractionReleased(context);
                currentInteractionHandler = null;
            }
        }


        private CardView GetViewFromCard(CardData card)
        {
            if (card == null) return null;
            CardView viewModel = card.GetComponent<CardView>();
            return viewModel != null ? viewModel : null;
        }

        private CardAnimationStateEnum GetStateTypeFromCard(CardData card)
        {
            if (card == null) return CardAnimationStateEnum.Unknown;
            CardView viewModel = card.GetComponent<CardView>();
            return viewModel.currentState;
        }
    }
}