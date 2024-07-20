using UnityEngine;

public sealed partial class LightAttackerDeathEffect : MonoBehaviour, IPooledObject<LightAttackerDeathEffect>
{
    [SerializeField]
    private Timer releaseToPoolTimer;

	public IPool<LightAttackerDeathEffect> ParentPool { get; set; }


	// Initialize
	public void OnTakenFromPool(IPool<LightAttackerDeathEffect> pool)
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

	public void OnReleaseToPool(IPool<LightAttackerDeathEffect> pool)
	{
		releaseToPoolTimer.Reset();
	}
}


#if UNITY_EDITOR

public sealed partial class LightAttackerDeathEffect
{ }

#endif