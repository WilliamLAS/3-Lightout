using UnityEngine;

public sealed partial class KillerObstacleDeathEffect : MonoBehaviour, IPooledObject<KillerObstacleDeathEffect>
{
    [SerializeField]
    private Timer releaseToPoolTimer;

	public IPool<KillerObstacleDeathEffect> ParentPool { get; set; }


	// Initialize
	public void OnTakenFromPool(IPool<KillerObstacleDeathEffect> pool)
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

	public void OnReleaseToPool(IPool<KillerObstacleDeathEffect> pool)
	{
		releaseToPoolTimer.Reset();
	}
}


#if UNITY_EDITOR

public sealed partial class KillerObstacleDeathEffect
{ }

#endif