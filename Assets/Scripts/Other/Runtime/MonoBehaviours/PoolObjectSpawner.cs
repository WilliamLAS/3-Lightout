using AYellowpaper;
using UnityEngine;

public sealed partial class PoolObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private InterfaceReference<IPool> pool;

    [SerializeField]
    private TimerRandomized spawnTimer;

    [SerializeField]
    [Tooltip("Only BoxCollider and SphereCollider are accepted")]
    private Collider spawnBounds;


	// Update
	private void Update()
	{
		if(spawnTimer.Tick())
		{
			var poolObject = pool.Value.GetUnknown();

			if (poolObject is Component spawnedComponent)
				spawnedComponent.transform.position = spawnBounds.GetRandomPoint();
			else
				pool.Value.ReleaseUnknown(poolObject);

			spawnTimer.ResetAndRandomize();
		}
	}
}


#if UNITY_EDITOR

public sealed partial class PoolObjectSpawner
{ }

#endif