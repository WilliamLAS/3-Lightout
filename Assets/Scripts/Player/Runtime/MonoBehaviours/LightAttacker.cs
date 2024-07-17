using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class LightAttacker : StateMachineDrivenPlayerBase
{
    [SerializeField]
    private Movable movementController;

    [SerializeField]
    private List<TargetType> acceptedTargetList;

    [SerializeField]
    private float stopFollowDistance;

	[SerializeField]
    private float followingOrbitDistance;

    [SerializeField]
    [Range(0f, 180f)]
    private float followingOrbitingAngleChangeSpeed;

    [NonSerialized]
    private Transform followTransform;

    private bool IsAbleToFollow => (movementController && followTransform);


	// Initialize
	protected override void OnEnable()
	{
        base.OnEnable();
	}


	// Update
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

		var newFollowPosition = followTransform.position;
        var norDirFollowingToSelf = followTransform.position.GetDifferenceTo(this.transform.position).normalized;

        newFollowPosition += (norDirFollowingToSelf * followingOrbitDistance);
        newFollowPosition = newFollowPosition.RotateByDegreeAngle(followingOrbitingAngleChangeSpeed, followTransform.position);

        if (this.transform.position.IsNearTo(newFollowPosition, stopFollowDistance))
            movementController.movingDirection = default;
        else
		    movementController.movingDirection = this.transform.position.GetDifferenceTo(newFollowPosition);
	}

	public void OnGrabTriggerEnter(Collider other)
    {
        if (!other)
            return;

        if (EventReflectorUtils.TryGetComponentByEventReflector<Target>(other.gameObject, out Target found))
        {
            if (found.CompareTag(Tag.Player))
                followTransform = found.transform;
        }
    }
}


#if UNITY_EDITOR

public sealed partial class LightAttacker
{ }

#endif