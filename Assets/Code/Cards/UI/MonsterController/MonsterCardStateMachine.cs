using Game.Cards.UI.State;
using Game.Lib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    public class MonsterCardStateMachine : InteractionStateMachine<MonsterCardState, MonsterCardStateEnum>
    {
        private CardAnimationComponents<MonsterCardState, MonsterCardStateEnum> components;
        private CardAnimationConfig config;

        public MonsterCardStateMachine(CardAnimationComponents<MonsterCardState, MonsterCardStateEnum> components, CardAnimationConfig config)
        {
            this.components = components;
            this.config = config;

            components.CardInfo.decorateCard.OnDestroy += OnDead;

            AddState(MonsterCardStateEnum.Idle, new MonsterCardIdle(components, config));
            AddState(MonsterCardStateEnum.Hover, new MonsterCardHover(components, config));
            AddState(MonsterCardStateEnum.Return, new MonsterCardReturn(components, config));
            AddState(MonsterCardStateEnum.Dead, new MonsterCardDead(components, config));
            
            // ChangeState(States[MonsterCardStateEnum.Return]);
        }

        public override void ProcessEvent(MonsterCardStateEnum stateType)
        {
            if( States.TryGetValue(stateType, out var state) )
            {
                ChangeState(state);
            }
        }

        public override void Update()
        {
            currentState?.OnUpdate();
        }

        public override void OnDestroy()
        {
            currentState?.OnExit();
        }

        public override void OnHoverEnter()
        {
            currentState?.OnHoverEnter();
        }

        public override void OnHoverExit()
        {
            currentState?.OnHoverExit();
        }

        public override void OnInteractPerformed(InputAction.CallbackContext context)
        {
            currentState?.OnClick(context);
        }

        public override void OnInteractCanceled(InputAction.CallbackContext context)
        {
            currentState?.OnRealise(context);
        }

        private void OnDead()
        {
            ProcessEvent(MonsterCardStateEnum.Dead);
            components.CardInfo.decorateCard.OnDestroy -= OnDead;
        }
    }
}