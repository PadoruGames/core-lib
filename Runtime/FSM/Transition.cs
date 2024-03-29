using System;

namespace Padoru.Core
{
	public class Transition<TState, TTrigger> where TState : Enum where TTrigger : Enum
	{
		public TState initialState;
		public TState targetState;
		public TTrigger trigger;

		public override bool Equals(object obj)
		{
			var otherTransition = (Transition<TState, TTrigger>)obj;

			return otherTransition.initialState.Equals(initialState) &&
				   otherTransition.targetState.Equals(targetState) &&
				   otherTransition.trigger.Equals(trigger);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}
