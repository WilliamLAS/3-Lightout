using UnityEngine;
using UnityEngine.Events;

public sealed partial class OnTriggerExitEvent : EventBase<UnityEvent<Collider>>
{
	// Update
	private void OnTriggerExit(Collider other)
	{
		onRaised?.Invoke(other);
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerExitEvent
{ }

#endif