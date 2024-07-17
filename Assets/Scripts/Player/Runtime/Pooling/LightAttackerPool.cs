using UnityEngine;

public sealed partial class LightAttackerPool : MonoBehaviourPoolBase<LightAttacker>
{
	[SerializeField]
	private LightAttacker prefab;


	// Initialize
	protected override LightAttacker OnCreatePooledObject()
	{
		return Instantiate(prefab);
	}


	// Update
	protected override void OnGetPooledObject(LightAttacker pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
	}


	// Dispose
	protected override void OnReleasePooledObject(LightAttacker pooledObject)
	{
		if (pooledObject)
			pooledObject.gameObject.SetActive(false);
	}

	protected override void OnDestroyPooledObject(LightAttacker pooledObject)
	{
		if (pooledObject)
			Destroy(pooledObject.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class LightAttackerPool
{ }

#endif