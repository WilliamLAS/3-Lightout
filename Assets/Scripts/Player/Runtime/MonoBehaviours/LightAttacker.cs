using FMODUnity;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class LightAttacker : StateMachineDrivenPlayerBase, IPooledObject<LightAttacker>, IMonoBehaviourPooledObject<LightAttacker>, IFrameDependentPhysicsInteractor<LightAttacker.PhysicsInteraction>
{
	public enum PhysicsInteraction
	{
		OnGrabTriggerEnter,
	}

	[Header("LightAttacker Movement")]
	#region LightAttacker Movement

	[SerializeField]
	private Movable movementController;

	[NonSerialized]
	private Player followingPlayer;


	#endregion

	[Header("LightAttacker Enemy")]
	#region Dark Enemy

	[SerializeField]
	private Enemy enemyController;


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

	private bool IsAbleToFollow => (movementController && enemyController && followingPlayer);


	#endregion

	[Header("LightAttacker Sounds")]
	#region LightAttacker Sounds

	[SerializeField]
	private EventReference deathEventReference;


	#endregion

	[Header("LightAttacker Visuals")]
	#region LightAttacker Visuals

	[SerializeField]
	private CinemachineImpulseSource cameraShaker;


	#endregion

	#region LightAttacker Following Orbiting

	[NonSerialized]
	private float orbitAngleChangeSpeed;

	[NonSerialized]
	private Vector3 orbitAxis;

	[NonSerialized]
	private float currentOrbitAngle;


	#endregion

	#region LightAttacker Other

	public IPool<LightAttacker> ParentPool
	{ get; set; }

	private Queue<FrameDependentInteraction<PhysicsInteraction>> FrameDependentInteractionQueue
	{ get; } = new();


	#endregion


	// Update
	protected override void Update()
	{
		DoFrameDependentPhysics();
		base.Update();
	}

	public void RegisterFrameDependentPhysicsInteraction(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!FrameDependentInteractionQueue.Contains(interaction))
			FrameDependentInteractionQueue.Enqueue(interaction);
	}

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
			orbitAxis = VectorUtils.RandomRange(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 0f));
			break;

			case (int)OrientationAxisType.XZ:
			orbitAxis = VectorUtils.RandomRange(new Vector3(0f, 0f, 0f), new Vector3(1f, 0f, 1f));
			break;

			case (int)OrientationAxisType.YZ:
			orbitAxis = VectorUtils.RandomRange(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 1f));
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
			}
		}
	}

	private void DoGrabTriggerEnter(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!interaction.collider)
			return;

		if (EventReflectorUtils.TryGetComponentByEventReflector<Player>(interaction.collider.gameObject, out Player found))
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

		// Try follow player or switch to attacking state
		if (enemyController.TryGetNearestEnemyTransform(out Transform nearestEnemyTransform))
		{
			State = PlayerStateType.Attacking;
			return;
		}

		currentOrbitAngle += (Time.deltaTime * orbitAngleChangeSpeed);
		currentOrbitAngle %= 360f;

		var orbitAxisAngle = Quaternion.AngleAxis(currentOrbitAngle, orbitAxis) * (Vector3.one - orbitAxis);
		var newFollowPosition = followingPlayer.transform.position - (orbitAxisAngle.normalized * followingOrbitDistance);

		// Follow
		if (this.transform.position.IsNearTo(newFollowPosition, stopFollowDistance))
			movementController.movingDirection = default;
		else
			movementController.movingDirection = this.transform.position.GetDifferenceTo(newFollowPosition);
	}

	protected override void DoAttackingState()
	{
		Transform nearestEnemyTransform = default;

		if (enemyController && !enemyController.TryGetNearestEnemyTransform(out nearestEnemyTransform))
		{
			State = PlayerStateType.Idle;
			return;
		}

		// Follow
		if (this.transform.position.IsNearTo(nearestEnemyTransform.position, stopFollowDistance))
			movementController.movingDirection = default;
		else
			movementController.movingDirection = this.transform.position.GetDifferenceTo(nearestEnemyTransform.position);
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

	protected override void OnStateChangedToIdle()
	{
		if (movementController)
			movementController.movingDirection = default;
	}

	protected override void OnStateChangedToFollowing()
	{
		RandomizeOrbit();
		base.OnStateChangedToFollowing();
	}

	protected override void OnStateChangedToDead()
	{
		cameraShaker.GenerateImpulse(0.5f);
		RuntimeManager.PlayOneShot(deathEventReference, this.transform.position);
		LightAttackerDeathEffectSingletonPool.Instance.Get(this.transform.position);
		ReleaseOrDestroySelf();
	}

	public void OnKilledOtherEnemy(Enemy killed)
	{
		State = PlayerStateType.Dead;
	}

	public void OnGotKilledByEnemy(Enemy killedBy)
	{
		State = PlayerStateType.Dead;
	}

	public void OnGrabTriggerEnter(Collider other)
		=> RegisterFrameDependentPhysicsInteraction(new(PhysicsInteraction.OnGrabTriggerEnter, other, null));

	public void OnTakenFromPool(IPool<LightAttacker> pool)
	{ }

	public void OnReleaseToPool(IPool<LightAttacker> pool)
	{ }


	// Dispose
	private void OnDisable()
	{
		if (GameControllerPersistentSingleton.IsQuitting || SceneControllerPersistentSingleton.IsActiveSceneChanging)
			return;

		if (followingPlayer)
		{
			followingPlayer.OnUnGrabbedLightAttacker(this);
			followingPlayer = null;
		}

		FrameDependentInteractionQueue.Clear();
	}

	public void ReleaseOrDestroySelf()
	{
		if (ParentPool != null)
			ParentPool.Release(this);
		else
			Destroy(this.gameObject);
	}

	public void CallExitInteractions()
	{
		throw new NotImplementedException();
	}
}


#if UNITY_EDITOR

public sealed partial class LightAttacker
{ }

#endif