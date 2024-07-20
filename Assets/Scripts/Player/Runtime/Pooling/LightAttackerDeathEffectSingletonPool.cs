using UnityEngine;

public sealed partial class LightAttackerDeathEffectSingletonPool : MonoBehaviourSingletonPoolBase<LightAttackerDeathEffectSingletonPool, LightAttackerDeathEffect>
{
	[SerializeField]
	private LightAttackerDeathEffect prefab;


	// Initialize
	protected override LightAttackerDeathEffect OnCreatePooledObject()
	{
		return Instantiate(prefab);
	}


	// Update
	protected override void OnGetPooledObject(LightAttackerDeathEffect pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
	}


	// Dispose
	protected override void OnReleasePooledObject(LightAttackerDeathEffect pooledObject)
	{
		if (pooledObject)
			pooledObject.gameObject.SetActive(false);
	}

	protected override void OnDestroyPooledObject(LightAttackerDeathEffect pooledObject)
	{
		if (pooledObject)
			Destroy(pooledObject.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class LightAttackerDeathEffectSingletonPool
{ }

#endif