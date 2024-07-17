using UnityEngine;
using UnityEngine.Events;

public sealed partial class OnCollisionEnterEvent : EventBase<UnityEvent<Collision>>
{
	// Update
	private void OnCollisionEnter(Collision collision)
	{
		onRaised?.Invoke(collision);
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionEnterEvent
{ }

#endif