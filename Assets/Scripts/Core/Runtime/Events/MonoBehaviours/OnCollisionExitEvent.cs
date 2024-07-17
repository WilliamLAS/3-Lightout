using UnityEngine;
using UnityEngine.Events;

public sealed partial class OnCollisionExitEvent : EventBase<UnityEvent<Collision>>
{
	// Update
	private void OnCollisionExit(Collision collision)
	{
		onRaised?.Invoke(collision);
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionExitEvent
{ }

#endif