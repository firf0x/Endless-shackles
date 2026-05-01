using System.Collections.Generic;
using Game.Lib;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Utils
{
    public class CursorInteraction : MonoBehaviour
    {
        [SerializeField] private InputActionAsset inputAction;
        [SerializeField] private LayerMask interactableLayer;

        private InputAction interactAction;
        private InputAction positionAction;
        private InteractionActionHandler currentHandler;

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
            CheckHover();
        }

        private void CheckHover()
        {
            Vector2 mouseScreenPos = positionAction.ReadValue<Vector2>();
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, Camera.main.nearClipPlane));

            RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero, 25f, interactableLayer);

            InteractionActionHandler newHandler = null;

            if (hit.collider != null)
            {
                newHandler = hit.collider.GetComponent<InteractionActionHandler>();
            }

            // Изменился
            if (currentHandler != newHandler)
            {
                // Переход
                if (currentHandler != null) currentHandler.OnCursorExit();
                
                currentHandler = newHandler;
                
                if (currentHandler != null) currentHandler.OnCursorEnter();
            }
        }

        private void OnInteractPerformed(InputAction.CallbackContext context)
        {
            if (currentHandler != null) currentHandler.OnInteract(context);
        }

        private void OnInteractCanceled(InputAction.CallbackContext context)
        {
            if (currentHandler != null) currentHandler.OnInteractReleased(context);
        }
    }
}