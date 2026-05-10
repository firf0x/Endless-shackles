using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Utils
{
    public sealed class InteractionHandler : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private InteractionBase interaction;

        public void InteractionPressed(InputAction.CallbackContext context) => interaction?.InteractionPressed(context);
        public void InteractionReleased(InputAction.CallbackContext context) => interaction?.InteractionReleased(context);
    }
}