using System;
using UnityEngine.InputSystem;

namespace Game.Utils
{
    [Serializable]
    public abstract class InteractionBase
    {
        public abstract void InteractionPressed(InputAction.CallbackContext context);
        public virtual void InteractionReleased(InputAction.CallbackContext context) { }
    }
}