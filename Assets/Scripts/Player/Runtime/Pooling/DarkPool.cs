using UnityEngine;

public sealed partial class DarkPool : MonoBehaviourPoolBase<Dark>
{
	[SerializeField]
	private Dark prefab;


	// Initialize
	protected override Dark OnCreatePooledObject()
	{
		return Instantiate(prefab);
	}


	// Update
	protected override void OnGetPooledObject(Dark pooledObject)
	{
		pooledObject.gameObject.SetActive(true);
	}


	// Dispose
	protected override void OnReleasePooledObject(Dark pooledObject)
	{
		if (pooledObject)
			pooledObject.gameObject.SetActive(false);
	}

	protected override void OnDestroyPooledObject(Dark pooledObject)
	{
		if (pooledObject)
			Destroy(pooledObject.gameObject);
	}
}


#if UNITY_EDITOR

public sealed partial class DarkPool
{ }

#endif