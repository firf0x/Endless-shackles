using UnityEngine.InputSystem;

namespace Game.Lib
{
    public interface InteractionActionHandler
    {
        void OnCursorEnter();
        void OnCursorExit();
        void OnInteract(InputAction.CallbackContext context);
        void OnInteractReleased(InputAction.CallbackContext context);
    }
}