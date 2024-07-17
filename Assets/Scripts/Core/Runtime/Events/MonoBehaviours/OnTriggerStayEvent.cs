using UnityEngine;
using UnityEngine.Events;

public sealed partial class OnTriggerStayEvent : EventBase<UnityEvent<Collider>>
{
	// Update
	private void OnTriggerStay(Collider other)
	{
		onRaised?.Invoke(other);
	}
}


#if UNITY_EDITOR

public sealed partial class OnTriggerStayEvent
{ }

#endif