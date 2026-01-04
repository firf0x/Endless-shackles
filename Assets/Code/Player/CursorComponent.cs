using DG.Tweening; // Не забудьте добавить эту директиву
using Game.Cards;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorComponent : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private LayerMask interactableLayer;
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
            if (hit.collider.TryGetComponent<CardData>(out card))
            {
                currentCard = card;
                cardStartPosition = currentCard.transform.position;
                isDragging = true;
                
                dragOffset = currentCard.transform.position - mouseWorldPos;
                dragOffset.z = 0;
                
                UpdateCardPosition();
            }
        }
    }

    private void StopDragging()
    {
        if (!isDragging) return;

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
}