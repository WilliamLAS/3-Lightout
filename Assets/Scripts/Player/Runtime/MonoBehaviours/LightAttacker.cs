using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class LightAttacker : StateMachineDrivenPlayerBase
{
	[Header("LightAttacker Movement")]
	#region LightAttacker Movement

	[SerializeField]
	private Movable movementController;

	[NonSerialized]
	private Player followingPlayer;


	#endregion

	[Header("LightAttacker Target")]
	#region LightAttacker Target

	[SerializeField]
    private List<TargetType> acceptedTargetList;


	#endregion

	[Header("LightAttacker Following")]
	#region LightAttacker Following

	[SerializeField]
	private float followingOrbitDistance;

	[SerializeField]
	private float stopFollowDistance;


	#endregion

	#region LightAttacker Following Orbiting

	[NonSerialized]
	private float orbitAngleChangeSpeed;

	[NonSerialized]
	private Vector3 orbitAxis;

	[NonSerialized]
	private float currentOrbitAngle;


	#endregion

	private bool IsAbleToFollow => (movementController && followingPlayer);


	// Initialize
	public void RandomizeOrbit()
	{
		// Randomize orbiting speed
		// Minimum 45 degree of speed is allowed
		orbitAngleChangeSpeed = UnityEngine.Random.Range(-180f, 180f);

		if (Math.Abs(orbitAngleChangeSpeed) < 45f)
			orbitAngleChangeSpeed += 45f * Mathf.Sign(orbitAngleChangeSpeed);

		// Randomize orbiting axis
		var orbitOrientationArray = Enum.GetValues(typeof(OrientationAxisType));
		var randomSelectedOrbitOrientation = UnityEngine.Random.Range(0, orbitOrientationArray.Length);

		switch (randomSelectedOrbitOrientation)
		{
			case (int)OrientationAxisType.XY:
			orbitAxis = VectorUtils.RandomRange(new Vector3(0f, 0f , 0f), new Vector3(1f, 1f, 0f));
			break;

			case (int)OrientationAxisType.XZ:
			orbitAxis = VectorUtils.RandomRange(new Vector3(0f, 0f , 0f), new Vector3(1f, 0f, 1f));
			break;

			case (int)OrientationAxisType.YZ:
			orbitAxis = VectorUtils.RandomRange(new Vector3(0f, 0f , 0f), new Vector3(0f, 1f, 1f));
			break;

			case (int)OrientationAxisType.YX:
				goto case (int)OrientationAxisType.XY;

			case (int)OrientationAxisType.ZX:
				goto case (int)OrientationAxisType.XZ;

			case (int)OrientationAxisType.ZY:
				goto case (int)OrientationAxisType.YZ;

			default:
				goto case (int)OrientationAxisType.XY;
		}
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

        currentOrbitAngle += (Time.deltaTime * orbitAngleChangeSpeed);
        currentOrbitAngle %= 360f;

        var orbitAxisAngle = Quaternion.AngleAxis(currentOrbitAngle, orbitAxis) * (Vector3.one - orbitAxis);
		var newFollowPosition = followingPlayer.transform.position - (orbitAxisAngle.normalized * followingOrbitDistance);

        if (this.transform.position.IsNearTo(newFollowPosition, stopFollowDistance))
            movementController.movingDirection = default;
        else
		    movementController.movingDirection = this.transform.position.GetDifferenceTo(newFollowPosition);
	}

	/*protected override void DoFollowingState()
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

		Debug.DrawLine(followTransform.position, newFollowPosition);

		if (this.transform.position.IsNearTo(newFollowPosition, stopFollowDistance))
			movementController.movingDirection = default;
		else
			movementController.movingDirection = this.transform.position.GetDifferenceTo(newFollowPosition);
	}*/

	protected override void OnStateChangedToFollowing()
	{
		RandomizeOrbit();
		base.OnStateChangedToFollowing();
	}

	public void OnGrabTriggerEnter(Collider other)
    {
        if (!other)
            return;

        if (EventReflectorUtils.TryGetComponentByEventReflector<Player>(other.gameObject, out Player found))
		{
			if (followingPlayer != found)
			{
				if (followingPlayer)
					followingPlayer.OnUnGrabbedLightAttacker(this);

				followingPlayer = found;
				followingPlayer.OnGrabbedLightAttacker(this);
			}
		}
    }


	// Dispose
	private void OnDisable()
	{
		if (followingPlayer)
		{
			followingPlayer.OnUnGrabbedLightAttacker(this);
			followingPlayer = null;
		}

		movementController.movingDirection = default;
	}
}


#if UNITY_EDITOR

public sealed partial class LightAttacker
{ }

#endif