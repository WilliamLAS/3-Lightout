using UnityEngine;
using UnityEngine.Events;

public sealed partial class OnCollisionStayEvent : EventBase<UnityEvent<Collision>>
{
	// Update
	private void OnCollisionStay(Collision collision)
	{
		onRaised?.Invoke(collision);
	}
}


#if UNITY_EDITOR

public sealed partial class OnCollisionStayEvent
{ }

#endif