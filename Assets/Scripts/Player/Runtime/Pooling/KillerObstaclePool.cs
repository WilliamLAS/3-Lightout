using UnityEngine;

public sealed partial class KillerObstaclePool : MonoBehaviourPoolBase<KillerObstacle>
{
	[SerializeField]
	private KillerObstacle prefab;


	// Initialize
	protected override KillerObstacle OnCreatePooledObject()
	{
		return Instantiate(prefab);
	}


	// Update
	protected override void OnGetPooledObject(KillerObstacle pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
	}


	// Dispose
	protected override void OnReleasePooledObject(KillerObstacle pooledObject)
	{
		if (pooledObject)
			pooledObject.gameObject.SetActive(false);
	}

	protected override void OnDestroyPooledObject(KillerObstacle pooledObject)
	{
		if (pooledObject)
			Destroy(pooledObject.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class KillerObstaclePool
{ }

#endif