using Game.Lib;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    public abstract class HandCardState : GenericState, GenericInteractionState
    {
        protected HandCardComponents components;
        protected HandCardConfig config;

        public HandCardState(HandCardComponents components, HandCardConfig config)
        {
            this.components = components;
            this.config = config;
        }

        public virtual void OnClick(InputAction.CallbackContext context) { }

        public virtual void OnRealise(InputAction.CallbackContext context) { }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        public virtual void OnUpdate() { }

        public virtual void OnClear() { }
    }
}