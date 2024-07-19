using UnityEngine;

public sealed partial class KillerObstacleSpawnSuspender : MonoBehaviour, IPooledObject<KillerObstacleSpawnSuspender>
{
    [SerializeField]
    private Timer releaseToPoolTimer;

	public IPool<KillerObstacleSpawnSuspender> ParentPool { get; set; }


	// Initialize
	public void OnTakenFromPool(IPool<KillerObstacleSpawnSuspender> pool)
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

	public void OnReleaseToPool(IPool<KillerObstacleSpawnSuspender> pool)
	{ }
}


#if UNITY_EDITOR

public sealed partial class KillerObstacleSpawnSuspender
{ }

#endif