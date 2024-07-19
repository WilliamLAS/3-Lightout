using UnityEngine;

public sealed partial class DarkSpawnSuspender : MonoBehaviour, IPooledObject<DarkSpawnSuspender>
{
    [SerializeField]
    private Timer releaseToPoolTimer;

	public IPool<DarkSpawnSuspender> ParentPool { get; set; }


	// Initialize
	public void OnTakenFromPool(IPool<DarkSpawnSuspender> pool)
	{ }


	// Update
	private void Update()
	{
		if (releaseToPoolTimer.Tick())
		{
			releaseToPoolTimer.Reset();
			ReleaseOrDestroySelf();
		}
	}


	// Dispose
	public void ReleaseOrDestroySelf()
	{
		if (ParentPool != null)
			ParentPool.Release(this);
		else
			Destroy(this.gameObject);
	}

	public void OnReleaseToPool(IPool<DarkSpawnSuspender> pool)
	{ }
}


#if UNITY_EDITOR

public sealed partial class DarkSpawnSuspender
{ }

#endif