using Game.Cards.UI.State;
using Game.Lib;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Cards.UI
{
    public class MonsterCardStateMachine : InteractionStateMachine<AnimationCardState, CardAnimationStateEnum>
    {
        private CardAnimationComponents components;
        private CardAnimationConfig config;

        public MonsterCardStateMachine(CardAnimationComponents components, CardAnimationConfig config)
        {
            this.components = components;
            this.config = config;

            components.CardInfo.decorateCard.OnDestroy += OnDead;

            AddState(CardAnimationStateEnum.Idle, new MonsterCardIdle(components, config));
            AddState(CardAnimationStateEnum.Hover, new MonsterCardHover(components, config));
            AddState(CardAnimationStateEnum.Return, new MonsterCardReturn(components, config));
            AddState(CardAnimationStateEnum.Dead, new MonsterCardDead(components, config));
            
            ChangeState(States[CardAnimationStateEnum.Return], CardAnimationStateEnum.Return);
        }

        public override void ProcessEvent(CardAnimationStateEnum stateType)
        {
            if( States.TryGetValue(stateType, out var state) )
            {
                ChangeState(state, stateType);
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
            ProcessEvent(CardAnimationStateEnum.Dead);
            components.CardInfo.decorateCard.OnDestroy -= OnDead;
        }
    }
}