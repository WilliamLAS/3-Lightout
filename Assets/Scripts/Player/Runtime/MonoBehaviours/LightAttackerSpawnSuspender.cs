using UnityEngine;

public sealed partial class LightAttackerSpawnSuspender : MonoBehaviour, IPooledObject<LightAttackerSpawnSuspender>
{
    [SerializeField]
    private Timer releaseToPoolTimer;

	public IPool<LightAttackerSpawnSuspender> ParentPool { get; set; }


	// Initialize
	public void OnTakenFromPool(IPool<LightAttackerSpawnSuspender> pool)
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

	public void OnReleaseToPool(IPool<LightAttackerSpawnSuspender> pool)
	{ }
}


#if UNITY_EDITOR

public sealed partial class LightAttackerSpawnSuspender
{ }

#endif