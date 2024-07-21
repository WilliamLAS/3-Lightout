using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

[DisallowMultipleComponent]
public sealed partial class Carryier : MonoBehaviour, IFrameDependentPhysicsInteractor<Carryier.PhysicsInteraction>
{
	public enum PhysicsInteraction
	{
		OnGrabTriggerEnter,
		OnGrabTriggerExit
	}

	[Header("Carryier Carry")]
	#region Carryier Carry

	public CarryierType carryierType;

	public int priority;

	public uint maxCarryableCount;

	private readonly List<Carryable> _carriedList = new();

	private ReadOnlyCollection<Carryable> _carriedReadonlyList;

	private List<Carryable> CarriedList
	{
		get
		{
			for (int i = _carriedList.Count - 1; i >= 0; i--)
			{
				if (!_carriedList[i])
					_carriedList.RemoveAt(i);
            }

			return _carriedList;
		}
	}

	public ReadOnlyCollection<Carryable> CarriedListReadonly
	{
		get
		{
			for (int i = _carriedList.Count - 1; i >= 0; i--)
			{
				if (!_carriedList[i])
					_carriedList.RemoveAt(i);
			}
			
			_carriedReadonlyList ??= new(_carriedList);
			return _carriedReadonlyList;
		}
	}

	public bool IsMaxCarryableCountExceeded => (CarriedList.Count >= maxCarryableCount);


	#endregion

	[Header("Carryier Events")]
	#region Carryable Events

	[SerializeField]
	private UnityEvent<Carryable> onGrabbed = new();

	[SerializeField]
	private UnityEvent<Carryable> onUngrabbed = new();


	#endregion

	#region Carryier Other

	private Queue<FrameDependentInteraction<PhysicsInteraction>> FrameDependentInteractionQueue
	{ get; } = new();


	#endregion


	// Update
	private void Update()
	{
		DoFrameDependentPhysics();
	}

	public bool TryGrabCarryable(Carryable requester)
	{
		if (IsMaxCarryableCountExceeded || !requester || CarriedList.Contains(requester))
			return false;

		_carriedList.Add(requester);
		var isGrabbed = requester.TryGetGrabbedBy(this);

		if (isGrabbed)
			onGrabbed?.Invoke(requester);
		else
			_carriedList.Remove(requester);

		return isGrabbed;
	}

	public void UngrabCarryable(Carryable requester)
	{
		if (CarriedList.Remove(requester))
		{
			requester.GetUngrabbed();
			onUngrabbed?.Invoke(requester);
		}
	}

	public void UngrabAll()
	{
        for (int i = CarriedList.Count - 1; i >= 0; i--)
        {
			var iteratedCarried = _carriedList[i];

			_carriedList.Remove(iteratedCarried);
			iteratedCarried.GetUngrabbed();
			onUngrabbed?.Invoke(iteratedCarried);
        }
	}

	public void RegisterFrameDependentPhysicsInteraction(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!FrameDependentInteractionQueue.Contains(interaction))
			FrameDependentInteractionQueue.Enqueue(interaction);
	}

	public void DoFrameDependentPhysics()
	{
		for (int i = FrameDependentInteractionQueue.Count; i > 0; i--)
		{
			var iteratedPhysicsInteraction = FrameDependentInteractionQueue.Dequeue();

			switch (iteratedPhysicsInteraction.interactionType)
			{
				case PhysicsInteraction.OnGrabTriggerEnter:
				DoGrabTriggerEnter(iteratedPhysicsInteraction);
				break;

				case PhysicsInteraction.OnGrabTriggerExit:
				DoGrabTriggerExit(iteratedPhysicsInteraction);
				break;
			}
		}
	}

	private void DoGrabTriggerEnter(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!interaction.collider)
			return;

		if (EventReflectorUtils.TryGetComponentByEventReflector<Carryable>(interaction.collider.gameObject, out Carryable found))
		{
			if (found.acceptedCarryiersList.Contains(carryierType))
				TryGrabCarryable(found);
		}
	}

	private void DoGrabTriggerExit(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!interaction.collider)
			return;

		if (EventReflectorUtils.TryGetComponentByEventReflector<Carryable>(interaction.collider.gameObject, out Carryable found))
			UngrabCarryable(found);
	}

	public void OnGrabTriggerEnter(Collider other)
		=> RegisterFrameDependentPhysicsInteraction(new(PhysicsInteraction.OnGrabTriggerEnter, other, null));

	public void OnGrabTriggerExit(Collider other)
		=> RegisterFrameDependentPhysicsInteraction(new(PhysicsInteraction.OnGrabTriggerExit, other, null));


	// Dispose
	private void OnDisable()
	{
		UngrabAll();
	}

	public void CallExitInteractions()
	{
		throw new System.NotImplementedException();
	}
}


#if UNITY_EDITOR

public sealed partial class Carryier
{ }

#endif

/*public bool TryGrabCarryable(Carryable requester)
{
	bool isCarriedValid = Carried;

	if (isCarriedValid || !requester || (requester == Carried))
		return false;

	var lastCarried = Carried;
	Carried = requester;
	var isGrabbed = requester.TryGetGrabbedBy(this);

	if (isGrabbed)
		onGrabbed?.Invoke(Carried);
	else
		Carried = lastCarried;

	return isGrabbed;
}

public void UngrabCarryable()
{
	if (Carried)
	{
		var lastCarryier = Carried;
		Carried = null;
		lastCarryier.GetUngrabbed();
		onUngrabbed?.Invoke(Carried);
	}
}*/
