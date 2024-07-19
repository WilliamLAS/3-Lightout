using UnityEngine;

public sealed partial class LightAttackerSpawnSuspenderPool : MonoBehaviourPoolBase<LightAttackerSpawnSuspender>
{
	[SerializeField]
	private LightAttackerSpawnSuspender prefab;


	// Initialize
	protected override LightAttackerSpawnSuspender OnCreatePooledObject()
	{
		return Instantiate(prefab);
	}


	// Update
	protected override void OnGetPooledObject(LightAttackerSpawnSuspender pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
	}


	// Dispose
	protected override void OnReleasePooledObject(LightAttackerSpawnSuspender pooledObject)
	{
		if (pooledObject)
			pooledObject.gameObject.SetActive(false);
	}

	protected override void OnDestroyPooledObject(LightAttackerSpawnSuspender pooledObject)
	{
		if (pooledObject)
			Destroy(pooledObject.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class LightAttackerSpawnSuspenderPool
{ }

#endif