using UnityEngine.Events;

public sealed partial class OnMonoMouseUpEvent : EventBase<UnityEvent>
{
	// Update
	private void OnMouseUp()
	{
		onRaised?.Invoke();
	}
}


#if UNITY_EDITOR

public sealed partial class OnMonoMouseUpEvent
{ }

#endif