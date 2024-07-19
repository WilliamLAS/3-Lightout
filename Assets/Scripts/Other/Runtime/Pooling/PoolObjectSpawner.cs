using AYellowpaper;
using System;
using System.Collections;
using UnityEngine;

public sealed partial class PoolObjectSpawner : MonoBehaviour
{
	[Header("PoolObjectSpawner Main Object")]
	#region PoolObjectSpawner Main Object

	[SerializeField]
    private InterfaceReference<IPool> mainObjectPool;

	public bool spawnByTimer;

	[SerializeField]
	private TimerRandomized spawnTimer;

	[SerializeField]
	private uint spawnAtStartCount;

	[SerializeField]
	private float spawnAtStartWaitForEachTimeInSeconds;

	[NonSerialized]
	private WaitForSeconds spawnAtStartEnumerator;


	#endregion

	[Header("PoolObjectSpawner Suspend Object")]
	#region PoolObjectSpawner Suspend Object

	[SerializeField]
	[Tooltip("Optional")]
	private InterfaceReference<IPool> suspendObjectPool;

	[SerializeField]
	private float suspendSpawningTimeInSeconds;


	#endregion

	[Header("PoolObjectSpawner Other")]
	#region PoolObjectSpawner Other

	[SerializeField]
    [Tooltip("Only BoxCollider and SphereCollider are accepted")]
    private Collider spawnBounds;


	#endregion


	// Initialize
	private void Awake()
	{
		spawnAtStartEnumerator = new (spawnAtStartWaitForEachTimeInSeconds);
	}

	private IEnumerator Start()
	{
        for (int i = 0; i < spawnAtStartCount; i++)
		{
			SpawnAtRandomPoint();
			yield return spawnAtStartEnumerator;
		}
    }


	// Update
	private void Update()
	{
		// Do spawn action once
		if(spawnByTimer && spawnTimer.Tick())
		{
			SpawnAtRandomPoint();
			spawnTimer.ResetAndRandomize();
		}
	}

	public void SpawnAtRandomPoint()
	{
		if (suspendObjectPool.UnderlyingValue)
			StartCoroutine(DoSuspendedSpawning(spawnBounds.GetRandomPoint(), suspendSpawningTimeInSeconds));
		else
			SpawnMainObjectFromPool(spawnBounds.GetRandomPoint());
	}

	private IEnumerator DoSuspendedSpawning(Vector3 worldPosition, float suspendSpawningTimeInSeconds)
	{
		SpawnSuspendObjectFromPool(worldPosition);

		var suspendSpawningTimer = new Timer(suspendSpawningTimeInSeconds);
		while (!suspendSpawningTimer.Tick())
			yield return null;

		// Spawn
		SpawnMainObjectFromPool(worldPosition);
	}

	public void SpawnMainObjectFromPool(Vector3 worldPosition)
	{
		var poolObject = mainObjectPool.Value.GetUnknown();

		if (poolObject is Component spawnedComponent)
			spawnedComponent.transform.position = worldPosition;
		else
			mainObjectPool.Value.ReleaseUnknown(poolObject);
	}

	public void SpawnSuspendObjectFromPool(Vector3 worldPosition)
	{
		var poolObject = suspendObjectPool.Value.GetUnknown();

		if (poolObject is Component spawnedComponent)
			spawnedComponent.transform.position = worldPosition;
		else
			suspendObjectPool.Value.ReleaseUnknown(poolObject);
	}
}


#if UNITY_EDITOR

public sealed partial class PoolObjectSpawner
{ }

#endif