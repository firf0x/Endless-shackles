using DG.Tweening;
using Game.Cards;
using Game.Deck;
using Game.Lib;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO: Сделать так чтобы курсор компонент просто вызывал методы у найденого объекта, а не только работал с картами.
// TODO: это нужно для того, чтобы сделать отображение информации о картах и других приколов.
public class CursorComponent : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask interactableLayer;
    [SerializeField] private LayerMask placeZoneLayer;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private InputActionAsset inputAction;

    [Header("Movement Settings")]
    [SerializeField] private float moveDuration = 0.3f;
    [SerializeField] private Ease moveEase = Ease.OutQuad;

    private InputAction interactAction;
    private InputAction positionAction;

    private CardData currentCard;
    private Vector3 cardStartPosition;
    private Vector3 dragOffset;
    private bool isDragging = false;
    private Tween moveTween;

    private void Awake()
    {
        var playerActionMap = inputAction.FindActionMap("Player");

        interactAction = playerActionMap.FindAction("Interact");
        positionAction = playerActionMap.FindAction("Mouse");

        interactAction.performed += OnInteractPerformed;
        interactAction.canceled += OnInteractCanceled;
    }

    private void OnEnable()
    {
        interactAction?.Enable();
        positionAction?.Enable();
    }

    private void OnDisable()
    {
        interactAction?.Disable();
        positionAction?.Disable();
        StopDragging();
    }

    private void OnDestroy()
    {
        if (interactAction != null)
        {
            interactAction.performed -= OnInteractPerformed;
            interactAction.canceled -= OnInteractCanceled;
        }
        
        moveTween?.Kill();
    }

    private void Update()
    {
        HandleDragging();
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        StartDragging();
    }

    private void OnInteractCanceled(InputAction.CallbackContext context)
    {
        StopDragging();
    }

    private void StartDragging()
    {
        if (isDragging) return;

        Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, raycastDistance, interactableLayer);

        if (hit.collider != null && hit.collider.gameObject.layer == 6)
        {
            CardData card;
            Debug.Log(!hit.collider.GetComponent<CardData>().decorateCard.isLocked);
            if (hit.collider.TryGetComponent<CardData>(out card) && !hit.collider.GetComponent<CardData>().decorateCard.isLocked)
            {
                if(card.decorateCard.Type == CardTypeEnum.Monster) return;
                
                currentCard = card;
                cardStartPosition = currentCard.transform.position;
                isDragging = true;
                
                dragOffset = currentCard.transform.position - mouseWorldPos;
                dragOffset.z = 0;
                
                UpdateCardPosition();
            }
        }

        if(hit.collider != null && hit.collider.TryGetComponent<DeckComponent>(out var deck))
        {
            GetCard(deck);
        }
    }

    private void StopDragging()
    {
        if (!isDragging) return;

        Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

        RaycastHit2D[] hits = Physics2D.RaycastAll(mouseWorldPos, Vector2.zero, raycastDistance);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.GetComponent<CardData>() != null && hit.collider.GetComponent<CardData>() != currentCard)
            {
                currentCard.Execute(hit.collider.gameObject);
                break;
            }

            if (hit.collider != null && hit.collider.gameObject.layer == 8)
            {
                string zoneTypeName = hit.collider.gameObject.name;
    
                // Debug.Log(zoneTypeName);
                
                // Проверяем является ли текущая карта картой защиты
                if (currentCard.TryGetCardFeature<CustomTypeDecorator<DefenceType>>(out var decorator))
                {
                    if (zoneTypeName == decorator.CustomType.ToString())
                    {
                        // Debug.Log("Карта добавилась");
                        DefendDeck.Instance.AddCard(currentCard);
                        HandDeck.Instance.RemoveCard(currentCard, false);
                        break;
                    }
                }
            }
        }

        isDragging = false;
        currentCard = null;
        
        moveTween?.Kill();
    }

    private void HandleDragging()
    {
        if (!isDragging || currentCard == null) return;
        
        UpdateCardPosition();
    }

    private void UpdateCardPosition()
    {
        Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));
        
        Vector3 targetPosition = mouseWorldPos + dragOffset;
        targetPosition.z = currentCard.transform.position.z;
        
        if (moveTween != null && moveTween.IsActive())
        {
            moveTween.Kill();
        }
        
        moveTween = currentCard.transform.DOMove(targetPosition, moveDuration).SetEase(moveEase).OnComplete(() => moveTween = null);
    }


    public void ForceStopDragging()
    {
        StopDragging();
    }
    
    public bool IsDraggingCard(out CardData draggedCard)
    {
        draggedCard = currentCard;
        return isDragging;
    }

    public void GetCard(DeckComponent deck) => deck.CreateNewCard();
}