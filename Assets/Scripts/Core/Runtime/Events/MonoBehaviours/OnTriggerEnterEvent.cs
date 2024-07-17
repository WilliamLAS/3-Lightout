using UnityEngine;
using UnityEngine.Events;

public sealed partial class OnTriggerEnterEvent : EventBase<UnityEvent<Collider>>
{
	// Update
	private void OnTriggerEnter(Collider other)
	{
		onRaised?.Invoke(other);
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerEnterEvent
{ }

#endif