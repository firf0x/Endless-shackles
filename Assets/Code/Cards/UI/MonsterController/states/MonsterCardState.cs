using Game.Lib;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    public abstract class MonsterCardState : GenericState, GenericInteractionState, GenericHoverState
    {
        protected CardAnimationComponents<MonsterCardState, MonsterCardStateEnum> components;
        protected CardAnimationConfig config;

        public MonsterCardState(CardAnimationComponents<MonsterCardState, MonsterCardStateEnum> components, CardAnimationConfig config)
        {
            this.components = components;
            this.config = config;
        }

        // Кнопка нажата
        public virtual void OnClick(InputAction.CallbackContext context) { }

        // Кнопка отпущена
        public virtual void OnRealise(InputAction.CallbackContext context) { }

        // Курсор над объектом
        public virtual void OnHoverEnter() { }
        
        // Курсор вышел из объекта
        public virtual void OnHoverExit() { }

        // Стартовый вызов состояния
        public virtual void OnEnter() { }

        // Конечный вызов состояния
        public virtual void OnExit() { }

        // Обновление [покадровое]
        public virtual void OnUpdate() { }
    }
}