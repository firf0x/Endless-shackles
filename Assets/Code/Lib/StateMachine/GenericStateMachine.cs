using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.InputSystem;

namespace Game.Lib
{
	public interface GenericStateMachine<out State, in TEnum> where TEnum : Enum
	{
		State currentState { get; }

		void ProcessEvent( TEnum stateType );
	}

	public interface GenericState
	{
		void OnEnter();
		void OnUpdate();
		void OnExit();
		void OnClear();
	}

	public interface GenericInteractionState
	{
		void OnClick(InputAction.CallbackContext context);
		void OnRealise(InputAction.CallbackContext context);

	}
}
