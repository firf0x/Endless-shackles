using System;
using System.Collections.Generic;

namespace Game.Lib
{
	public abstract class UpdaterStateMachine<State, TEnum> : IDisposable, GenericStateMachine<State, TEnum> where State : GenericState where TEnum : Enum
	{
		public State currentState { get; protected set; }
		public TEnum currentType { get; protected set; }

		private Dictionary<TEnum, State> states { get; set; } = new Dictionary<TEnum, State>();
		protected IReadOnlyDictionary<TEnum, State> States => states;

        public event Action<State, State> OnChangeState;

        public abstract void ProcessEvent( TEnum stateType );

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

			var oldState = currentState;

			oldState?.OnExit();
			currentState = newState;
			currentState?.OnEnter();
			
			OnChangeState?.Invoke(oldState, currentState);
		}

		public void Dispose()
		{
			states.Clear();
		}
	}
}
