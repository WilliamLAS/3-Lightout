using UnityEngine;
using UnityEngine.Pool;

public abstract partial class MonoBehaviourSingletonPoolBase<SingletonType, PooledObjectType> : MonoBehaviourSingletonBase<SingletonType>, IPool<PooledObjectType>
    where SingletonType : MonoBehaviourSingletonPoolBase<SingletonType, PooledObjectType>
    where PooledObjectType : class
{
    [SerializeField]
	[Tooltip("Collection checks will throw errors if we try to release an item that is already in the pool. Disable if optimization is preferred and you are sure about your system stability")]
    private bool collectionCheck = true;

	[field: SerializeField]
	public int MaxPoolSize { get; private set; } = 100;

	public ObjectPool<PooledObjectType> MainPool { get; private set; }


	// Initialize
	protected override void Awake()
	{
		MainPool = new ObjectPool<PooledObjectType>(OnCreatePooledObject, OnGetPooledObject_Internal, OnReleasePooledObject_Internal, OnDestroyPooledObject, collectionCheck, 10, MaxPoolSize);
		base.Awake();
	}

	protected abstract PooledObjectType OnCreatePooledObject();

	private void OnGetPooledObject_Internal(PooledObjectType pooledObject)
	{
		if (pooledObject is IPooledObject<PooledObjectType> foundObject)
		{
			foundObject.ParentPool = this;
			foundObject.OnTakenFromPool(this);
		}

		OnGetPooledObject(pooledObject);
	}

	protected abstract void OnGetPooledObject(PooledObjectType pooledObject);


	// Update
	public PooledObjectType Get()
		=> MainPool.Get();

	public object GetUnknown()
		=> Get();

	public PooledObjectType Get(Vector3 worldPosition)
	{
		var pooledObject = MainPool.Get();

		if (pooledObject is MonoBehaviour pooledMonoBehaviour)
			pooledMonoBehaviour.transform.position = worldPosition;
		else
			Debug.LogErrorFormat("{0} is not a type of MonoBehaviour. Returned normal pooled object", typeof(PooledObjectType));

		return pooledObject;
	}

	public PooledObject<PooledObjectType> Get(out PooledObjectType pooledObject)
		=> MainPool.Get(out pooledObject);

	public PooledObject<PooledObjectType> Get(Vector3 worldPosition, out PooledObjectType pooledObject)
	{
		var disposablePooledObject = MainPool.Get(out PooledObjectType takenPooledObject);
		pooledObject = takenPooledObject;

		if (takenPooledObject is MonoBehaviour pooledMonoBehaviour)
			pooledMonoBehaviour.transform.position = worldPosition;
		else
			Debug.LogErrorFormat("{0} is not a type of MonoBehaviour. Returned normal pooled object", typeof(PooledObjectType));

		return disposablePooledObject;
	}

	public void Release(PooledObjectType obj)
		=> MainPool.Release(obj);

	public void ReleaseUnknown(object obj)
		=> Release(obj as PooledObjectType);

	public void Clear()
		=> MainPool.Clear();


	// Dispose
	protected virtual void OnDestroy()
	{
		Clear();
	}

	protected abstract void OnDestroyPooledObject(PooledObjectType pooledObject);

	private void OnReleasePooledObject_Internal(PooledObjectType pooledObject)
	{
		if (pooledObject is IPooledObject<PooledObjectType> foundObject)
			foundObject.OnReleaseToPool(this);

		OnReleasePooledObject(pooledObject);
	}

	protected abstract void OnReleasePooledObject(PooledObjectType pooledObject);
}


#if UNITY_EDITOR

public abstract partial class MonoBehaviourSingletonPoolBase<SingletonType, PooledObjectType>
{ }

#endif