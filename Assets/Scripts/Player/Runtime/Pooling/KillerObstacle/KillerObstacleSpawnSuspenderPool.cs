using UnityEngine;

public sealed partial class KillerObstacleSpawnSuspenderPool : MonoBehaviourPoolBase<KillerObstacleSpawnSuspender>
{
	[SerializeField]
	private KillerObstacleSpawnSuspender prefab;


	// Initialize
	protected override KillerObstacleSpawnSuspender OnCreatePooledObject()
	{
		return Instantiate(prefab);
	}


	// Update
	protected override void OnGetPooledObject(KillerObstacleSpawnSuspender pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
	}


	// Dispose
	protected override void OnReleasePooledObject(KillerObstacleSpawnSuspender pooledObject)
	{
		if (pooledObject)
			pooledObject.gameObject.SetActive(false);
	}

	protected override void OnDestroyPooledObject(KillerObstacleSpawnSuspender pooledObject)
	{
		if (pooledObject)
			Destroy(pooledObject.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class KillerObstacleSpawnSuspenderPool
{ }

#endif