using System;
using System.Collections.Generic;

namespace Game.Lib
{
	public abstract class InteractionStateMachine<State, TEnum> : IDisposable, GenericStateMachine<State, TEnum> where State : GenericState where TEnum : Enum
	{
		public State currentState { get; protected set; }

		private Dictionary<TEnum, State> states { get; set; } = new Dictionary<TEnum, State>();
		protected IReadOnlyDictionary<TEnum, State> States => states;

		public abstract void ProcessEvent( TEnum stateType );

		public abstract void OnHoverEnter();
		public abstract void OnHoverExit();

		public abstract void OnInteractPerformed();
		public abstract void OnInteractCanceled();

		// Стандартные методы
		public abstract void Update();
		public abstract void OnDestroy();

		protected void AddState(TEnum @enum, State @state)
		{
			states.Add(@enum, state);
		}

		protected void ChangeState( State newState )
		{
			if ( !states.ContainsValue(newState) || newState == null ) return;

			if( currentState == null )
            {
				currentState = newState;
				currentState?.OnEnter();
				return;
            }

			if ( currentState.Equals( newState ) ) return;

			currentState?.OnExit();
			currentState = newState;
			currentState?.OnEnter();
		}

		public void Dispose()
		{
			states.Clear();
		}
	}
}
