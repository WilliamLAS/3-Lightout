using System;
using UnityEngine;
using UnityEngine.Events;

public abstract partial class LevelProgressControllerSingletonBase<SingletonType> : MonoBehaviourSingletonBase<SingletonType>
	where SingletonType : LevelProgressControllerSingletonBase<SingletonType>
{
	[Header("LevelProgressControllerSingletonBase Events")]
	#region LevelProgressControllerSingletonBase Events

	[SerializeField]
	protected UnityEvent onFinishedLevel = new();


	#endregion

	#region LevelProgressControllerSingletonBase Progress

	[NonSerialized]
	protected float _levelProgress;

	public float LevelProgress => _levelProgress;

	public bool IsFinishedLevel
	{ get; protected set; }

	#endregion


	// Update
	protected virtual void Update()
	{
		CheckProgressState();
	}

	private void CheckProgressState()
	{
		if (!IsFinishedLevel && _levelProgress >= 0.995f)
		{
			IsFinishedLevel = true;
			onFinishedLevel?.Invoke();
		}
	}

	protected abstract void UpdateProgress();
}


#if UNITY_EDITOR

public abstract partial class LevelProgressControllerSingletonBase<SingletonType>
{ }

#endif