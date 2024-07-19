using UnityEngine;

public sealed partial class DarkSpawnSuspenderPool : MonoBehaviourPoolBase<DarkSpawnSuspender>
{
	[SerializeField]
	private DarkSpawnSuspender prefab;


	// Initialize
	protected override DarkSpawnSuspender OnCreatePooledObject()
	{
		return Instantiate(prefab);
	}


	// Update
	protected override void OnGetPooledObject(DarkSpawnSuspender pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
	}


	// Dispose
	protected override void OnReleasePooledObject(DarkSpawnSuspender pooledObject)
	{
		if (pooledObject)
			pooledObject.gameObject.SetActive(false);
	}

	protected override void OnDestroyPooledObject(DarkSpawnSuspender pooledObject)
	{
		if (pooledObject)
			Destroy(pooledObject.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class DarkSpawnSuspenderPool
{ }

#endif