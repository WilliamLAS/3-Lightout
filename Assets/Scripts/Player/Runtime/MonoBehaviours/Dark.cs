using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class Dark : StateMachineDrivenPlayerBase
{
	[Header("Dark Movement")]
	#region Dark Movement

	[SerializeField]
	private Movable movementController;


	#endregion

	[Header("Dark Target")]
	#region Dark Target

	[SerializeField]
    private List<TargetType> acceptedTargetTypeList;

	[NonSerialized]
	private readonly HashSet<Target> targetInRangeSet = new();


	#endregion

	private bool IsAbleToFollow => (movementController && (targetInRangeSet.Count > 0));


	// Initialize


	// Update
	private bool TryGetNearestTargetTransform(out Transform nearestTransform)
	{
		nearestTransform = default;
		var cachedTransformList = ListPool<Transform>.Get();

		foreach (var iteratedTarget in targetInRangeSet)
		{
			if (acceptedTargetTypeList.Contains(iteratedTarget.TargetType))
				cachedTransformList.Add(iteratedTarget.transform);
		}

		var isFoundNearestTransform = this.transform.TryGetNearestTransform(cachedTransformList.GetEnumerator(), out nearestTransform);
		ListPool<Transform>.Release(cachedTransformList);

		return isFoundNearestTransform;
	}

	protected override void DoIdleState()
	{
        if (!IsAbleToFollow)
        {
		    base.DoIdleState();
            return;
        }

        State = PlayerStateType.Following;
	}

	protected override void DoFollowingState()
	{
        if (!IsAbleToFollow)
        {
            State = PlayerStateType.Idle;
            return;
        }

		if (TryGetNearestTargetTransform(out Transform nearestTransform))
			movementController.movingDirection = this.transform.position.GetDifferenceTo(nearestTransform.position);
		else
			movementController.movingDirection = default;
	}

	public void OnTargetTriggerEnter(Collider other)
	{
		if (!other)
			return;

		if (EventReflectorUtils.TryGetComponentByEventReflector<Target>(other.gameObject, out Target found))
			targetInRangeSet.Add(found);
	}

	public void OnTargetTriggerExit(Collider other)
	{
		if (!other)
		{
			targetInRangeSet.RemoveWhere((iteratedTarget) => !iteratedTarget);
			return;
		}

		if (EventReflectorUtils.TryGetComponentByEventReflector<Target>(other.gameObject, out Target found))
			targetInRangeSet.Remove(found);
	}


	// Dispose
	private void OnDisable()
	{
		movementController.movingDirection = default;
	}
}


#if UNITY_EDITOR

public sealed partial class Dark
{ }

#endif