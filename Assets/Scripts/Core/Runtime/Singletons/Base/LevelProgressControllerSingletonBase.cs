using System;
using UnityEngine;
using UnityEngine.Events;

public abstract partial class LevelProgressControllerSingletonBase<SingletonType> : MonoBehaviourSingletonBase<SingletonType>
	where SingletonType : LevelProgressControllerSingletonBase<SingletonType>
{
	[Header("LevelProgressControllerSingletonBase Events")]
	#region LevelProgressControllerSingletonBase Events

	[SerializeField]
	protected UnityEvent onProgressFinished = new();


	#endregion

	#region LevelProgressControllerSingletonBase Progress

	[NonSerialized]
	protected float _levelProgress;

	public float LevelProgress => _levelProgress;

	public bool IsProgressFinished
	{ get; protected set; }

	#endregion


	// Update
	protected virtual void Update()
	{
		CheckProgressState();
	}

	protected virtual void CheckProgressState()
	{
		if (!IsProgressFinished && _levelProgress >= 0.995f)
		{
			IsProgressFinished = true;
			onProgressFinished?.Invoke();
		}
	}

	protected abstract void UpdateProgress();
}


#if UNITY_EDITOR

public abstract partial class LevelProgressControllerSingletonBase<SingletonType>
{ }

#endif