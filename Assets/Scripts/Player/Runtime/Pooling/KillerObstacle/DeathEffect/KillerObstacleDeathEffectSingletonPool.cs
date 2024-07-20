using UnityEngine;

public abstract partial class KillerObstacleDeathEffectSingletonPool<SingletonType> : MonoBehaviourSingletonPoolBase<SingletonType, KillerObstacleDeathEffect>
	where SingletonType : KillerObstacleDeathEffectSingletonPool<SingletonType>
{
	[SerializeField]
	private KillerObstacleDeathEffect prefab;


	// Initialize
	protected override KillerObstacleDeathEffect OnCreatePooledObject()
	{
		return Instantiate(prefab);
	}


	// Update
	protected override void OnGetPooledObject(KillerObstacleDeathEffect pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
	}


	// Dispose
	protected override void OnReleasePooledObject(KillerObstacleDeathEffect pooledObject)
	{
		if (pooledObject)
			pooledObject.gameObject.SetActive(false);
	}

	protected override void OnDestroyPooledObject(KillerObstacleDeathEffect pooledObject)
	{
		if (pooledObject)
			Destroy(pooledObject.gameObject);
	}
}


#if UNITY_EDITOR

public abstract partial class KillerObstacleDeathEffectSingletonPool<SingletonType>
{ }

#endif