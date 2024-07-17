using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class Dark : StateMachineDrivenPlayerBase, IPooledObject<Dark>, IMonoBehaviourPooledObject<Dark>
{
	[Header("Dark Movement")]
	#region Dark Movement

	[SerializeField]
	private Movable movementController;


	#endregion

	[Header("Dark Enemy")]
	#region Dark Enemy

	[SerializeField]
	private Enemy enemyController;


	#endregion

	#region Dark Following

	private bool IsAbleToFollow => (movementController && enemyController && enemyController.IsAcceptedTargetFoundInRange());


	#endregion

	#region Dark Other

	public IPool<Dark> ParentPool
	{ get; set; }


	#endregion


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

		if (enemyController.TryGetNearestEnemyTransform(out Transform nearestTransform))
			movementController.movingDirection = this.transform.position.GetDifferenceTo(nearestTransform.position);
		else
			movementController.movingDirection = default;
	}

	protected override void OnStateChangedToIdle()
	{
		if (movementController)
			movementController.movingDirection = default;
	}

	protected override void OnStateChangedToDead()
	{
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

	public void OnTakenFromPool(IPool<Dark> pool)
	{ }

	public void OnReleaseToPool(IPool<Dark> pool)
	{ }


	// Dispose
	private void OnDisable()
	{
		movementController.movingDirection = default;
	}

	public void ReleaseOrDestroySelf()
	{
		if (ParentPool != null)
			ParentPool.Release(this);
		else
			Destroy(this.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class Dark
{ }

#endif
