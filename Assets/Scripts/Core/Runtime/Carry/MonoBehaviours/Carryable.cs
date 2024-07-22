using System;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public sealed partial class Carryable : MonoBehaviour
{
	[Header("Carryable Carry")]
	#region Carryable Carry

	public List<CarryierType> acceptedCarryiersList = new();

	[field: NonSerialized]
	public Carryier Carryier
	{ get; private set; }


	#endregion

	[Header("Carryable Events")]
	#region Carryable events

	[SerializeField]
	private UnityEvent<Carryier> onGrabben = new();

	[SerializeField]
	private UnityEvent<Carryier> onUngrabben = new();


	#endregion


	// Update
	private void Update()
	{
		UpdateStateByCarried();
	}

	public bool TryGetGrabbedBy(Carryier requester)
	{
		if (!requester || (requester == Carryier))
			return false;

		var isAbleToGetGrabbed = IsAbleToGetGrabbedBy(requester);
		if (isAbleToGetGrabbed)
		{
			if (Carryier)
				GetUngrabbed();

			Carryier = requester;
			requester.TryGrabCarryable(this);
			onGrabben?.Invoke(Carryier);
		}
		
		return isAbleToGetGrabbed;
	}

	public void GetUngrabbed()
	{
		if (Carryier)
		{
			var lastCarryier = Carryier;
			Carryier = null;
			lastCarryier.UngrabCarryable(this);
			onUngrabben?.Invoke(Carryier);
		}
	}

	public bool IsAbleToGetGrabbedBy(Carryier carrier)
	{
		if (!enabled || !this.gameObject.activeSelf)
			return false;

		bool isCarryierValid = Carryier;
		var isPriorityHigherThanCurrent = isCarryierValid && (carrier.priority > Carryier.priority);
		var isCloserThanCurrent = isCarryierValid && (carrier.transform.position - this.transform.position).sqrMagnitude < (Carryier.transform.position - this.transform.position).sqrMagnitude;

		return !isCarryierValid || (!carrier.IsMaxCarryableCountExceeded && (isPriorityHigherThanCurrent || isCloserThanCurrent));
	}

	private void UpdateStateByCarried()
	{
		// Check if carryier is available, If reference is still valid but it is disabled, call onUnGrabbed in Carryier.
		if (Carryier && (!Carryier.gameObject.activeSelf || !Carryier.enabled))
			GetUngrabbed();
	}


	// Dispose
	private void OnDisable()
	{
		GetUngrabbed();
	}
}


#if UNITY_EDITOR

public sealed partial class Carryable
{ }

#endif


/*public bool TryGetGrabbedBy(Carryier requester)
{
	bool isCarryierValid = Carryier;

	if (isCarryierValid || !requester || (requester == Carryier))
		return false;

	var isPriorityHigherThanCurrent = (requester.priority > Carryier.priority);
	var isCloserThanCurrent = isCarryierValid && (requester.transform.position - this.transform.position).sqrMagnitude < (Carryier.transform.position - this.transform.position).sqrMagnitude;
	var isAbleToGetGrabbed = (!isCarryierValid || isPriorityHigherThanCurrent || isCloserThanCurrent);

	if (isAbleToGetGrabbed)
	{
		Carryier = requester;
		requester.TryGrabCarryable(this);
		onGrabben?.Invoke(Carryier);
	}

	return isAbleToGetGrabbed;
}

public void GetUngrabbed()
{
	if (Carryier)
	{
		var lastCarryier = Carryier;
		Carryier = null;
		lastCarryier.UngrabCarryable();
		onUngrabben?.Invoke(Carryier);
	}
}

private void UpdateStateByCarried()
{
	// Check if carryier is available, If reference is still valid but it is disabled, call onUnGrabbed in Carryier.
	if (Carryier && !Carryier.gameObject.activeSelf || !Carryier.enabled)
		Carryier.UngrabCarryable();
}*/