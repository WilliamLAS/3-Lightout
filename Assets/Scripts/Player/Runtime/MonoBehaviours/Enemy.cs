using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public sealed partial class Enemy : MonoBehaviour, IFrameDependentPhysicsInteractor<Enemy.PhysicsInteraction>
{
	public enum PhysicsInteraction
	{
		OnEnemyTriggerEnter,
		OnEnemyTriggerExit,
		OnEnemyKillTriggerEnter
	}

	[Header("Enemy Target")]
	#region Enemy Target

	public TargetType targetType;

	public List<TargetType> acceptedTargetTypeList = new();

	[NonSerialized]
	private readonly HashSet<Enemy> _targetInRangeSet = new();

	[NonSerialized]
	private ReadOnlySet<Enemy> _targetInRangeReadonlySet;

	private HashSet<Enemy> TargetInRangeSet
	{
		get
		{
			_targetInRangeSet.RemoveWhere(iteratedEnemy => (!iteratedEnemy || !iteratedEnemy.gameObject.activeSelf));
			return _targetInRangeSet;
		}
	}

	public ReadOnlySet<Enemy> TargetInRangeReadonlySet
	{
		get
		{
			_targetInRangeReadonlySet ??= new (_targetInRangeSet);
			_targetInRangeSet.RemoveWhere(iteratedEnemy => (!iteratedEnemy || !iteratedEnemy.gameObject.activeSelf));
			return _targetInRangeReadonlySet;
		}
	}


	#endregion

	[Header("Enemy Events")]
	#region Enemy Events

	[SerializeField]
	private UnityEvent<Enemy> onGotKilledByEnemy = new();

	[SerializeField]
	private UnityEvent<Enemy> onKilledOtherEnemy = new();


	#endregion

	#region Enemy Stats

	public bool IsDead
	{ get; private set; }


	#endregion

	#region Enemy Other

	private Queue<FrameDependentInteraction<PhysicsInteraction>> FrameDependentInteractionQueue
	{ get; } = new();


	#endregion


	// Initialize
	private void OnEnable()
	{
		IsDead = false;
	}


	// Update
	private void Update()
	{
		DoFrameDependentPhysics();
	}

	public void RegisterFrameDependentPhysicsInteraction(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!FrameDependentInteractionQueue.Contains(interaction))
			FrameDependentInteractionQueue.Enqueue(interaction);
	}

	// TODO: Refactor in next versions
	public bool TryGetNearestEnemyTransform(out Transform nearestTransform)
	{
		nearestTransform = default;
		var cachedTransformList = ListPool<Transform>.Get();

		foreach (var iteratedTarget in TargetInRangeSet)
		{
			if (acceptedTargetTypeList.Contains(iteratedTarget.targetType))
				cachedTransformList.Add(iteratedTarget.transform);
		}

		var isFoundNearestTransform = this.transform.TryGetNearestTransform(cachedTransformList.GetEnumerator(), out nearestTransform);
		ListPool<Transform>.Release(cachedTransformList);

		return isFoundNearestTransform;
	}

	public bool IsAcceptedTargetFoundInRange()
	{
		foreach (var iteratedTarget in TargetInRangeSet)
		{
			if (acceptedTargetTypeList.Contains(iteratedTarget.targetType))
				return true;
		}

		return false;
	}

	public void DoFrameDependentPhysics()
	{
		for (int i = FrameDependentInteractionQueue.Count; i > 0; i--)
		{
			var iteratedPhysicsInteraction = FrameDependentInteractionQueue.Dequeue();

			switch (iteratedPhysicsInteraction.interactionType)
			{
				case PhysicsInteraction.OnEnemyTriggerEnter:
				DoEnemyTriggerEnter(iteratedPhysicsInteraction);
				break;

				case PhysicsInteraction.OnEnemyTriggerExit:
				DoEnemyTriggerExit(iteratedPhysicsInteraction);
				break;

				case PhysicsInteraction.OnEnemyKillTriggerEnter:
				DoEnemyKillTriggerEnter(iteratedPhysicsInteraction);
				break;
			}
		}
	}

	private void DoEnemyTriggerEnter(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!interaction.collider)
			return;

		if (EventReflectorUtils.TryGetComponentByEventReflector<Enemy>(interaction.collider.gameObject, out Enemy found))
			TargetInRangeSet.Add(found);
	}

	private void DoEnemyTriggerExit(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!interaction.collider)
			return;

		if (EventReflectorUtils.TryGetComponentByEventReflector<Enemy>(interaction.collider.gameObject, out Enemy found))
			TargetInRangeSet.Remove(found);
	}

	private void DoEnemyKillTriggerEnter(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!interaction.collider)
			return;

		if (EventReflectorUtils.TryGetComponentByEventReflector<Enemy>(interaction.collider.gameObject, out Enemy found))
		{
			if (acceptedTargetTypeList.Contains(found.targetType) && !found.IsDead)
			{
				onKilledOtherEnemy?.Invoke(found);
				found.OnGotKilledByEnemy(this);
			}
		}
	}

	public void OnKilledOtherEnemy(Enemy killed)
	{
		onKilledOtherEnemy?.Invoke(killed);
	}

	public void OnGotKilledByEnemy(Enemy killedBy)
	{
		IsDead = true;
		onGotKilledByEnemy?.Invoke(killedBy);
	}

	public void OnEnemyTriggerEnter(Collider other)
		=> RegisterFrameDependentPhysicsInteraction(new (PhysicsInteraction.OnEnemyTriggerEnter, other, null));

	public void OnEnemyTriggerExit(Collider other)
		=> RegisterFrameDependentPhysicsInteraction(new (PhysicsInteraction.OnEnemyTriggerExit, other, null));

	public void OnEnemyKillTriggerEnter(Collider other)
		=> RegisterFrameDependentPhysicsInteraction(new (PhysicsInteraction.OnEnemyKillTriggerEnter, other, null));


	// Dispose
	private void OnDisable()
	{
		CallExitInteractions();
		FrameDependentInteractionQueue.Clear();
	}

	public void CallExitInteractions()
	{
		TargetInRangeSet.Clear();
	}
}


#if UNITY_EDITOR

public sealed partial class Enemy
{ }

#endif