using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public sealed partial class ScreenControllerSingleton : MonoBehaviourSingletonBase<ScreenControllerSingleton>
{
	[Header("ScreenControllerSingleton Explosion")]
	#region ScreenControllerSingleton Explosion

	[SerializeField]
	private Volume _explosionVolume;

	public Volume ExplosionVolume => _explosionVolume;


	#endregion

	[Header("ScreenControllerSingleton Fade")]
	#region ScreenControllerSingleton Fade

	[SerializeField]
	private Volume _fadeVolume;

	public Volume FadeVolume => _fadeVolume;


	#endregion

	#region ScreenControllerSingleton Volume

	[NonSerialized]
	private readonly Dictionary<Volume, Coroutine> activeControlledVolumeWeightDict = new();


	#endregion


	// Update
	public void DoExplosion(float endTimeInSeconds = 1f, Action onExplosionEnded = null)
		=> ChangeVolumeWeight(_explosionVolume, 1f, 0f, endTimeInSeconds, onExplosionEnded);

	public void DoFade(float desiredFadeAmount, float fadeOutTimeInSeconds = 1f, float startFadeAmount = -1f, Action onFadeEnded = null)
	{
		if (startFadeAmount == -1f)
			startFadeAmount = _fadeVolume.weight;

		ChangeVolumeWeight(_fadeVolume, startFadeAmount, desiredFadeAmount, fadeOutTimeInSeconds, onFadeEnded);
	}

	public Coroutine ChangeVolumeWeight(Volume volume, float startWeight, float targetWeight, float endTimeInSeconds = 1f, Action onEnded = null)
	{
		// Stop and remove old volume coroutine
		if (activeControlledVolumeWeightDict.ContainsKey(volume))
		{
			var lastVolumeCoroutine = activeControlledVolumeWeightDict[volume];

			if (lastVolumeCoroutine != null)
				StopCoroutine(lastVolumeCoroutine);

			activeControlledVolumeWeightDict.Remove(volume);
		}

		var volumeWeightCoroutine = StartCoroutine(ChangeVolumeweight_Internal(volume, startWeight, targetWeight, endTimeInSeconds, onEnded));
		activeControlledVolumeWeightDict.Add(volume, volumeWeightCoroutine);
		return volumeWeightCoroutine;
	}

	private IEnumerator ChangeVolumeweight_Internal(Volume volume, float startWeight, float targetWeight, float endTimeInSeconds, Action onEnded)
	{
		startWeight = Mathf.Clamp01(startWeight);
		targetWeight = Mathf.Clamp01(targetWeight);
		var fadeOutTimer = new Timer(endTimeInSeconds);

		// TODO: Timer progress used here
		while (!fadeOutTimer.HasEnded)
		{
			fadeOutTimer.Tick();
			var timerProgress = (fadeOutTimer.TickSecond - fadeOutTimer.CurrentSecond) / fadeOutTimer.TickSecond;

			volume.weight = Mathf.Lerp(startWeight, targetWeight, timerProgress);
			yield return null;
		}

		onEnded?.Invoke();
		activeControlledVolumeWeightDict.Remove(volume);
	}
}


#if UNITY_EDITOR

public sealed partial class ScreenControllerSingleton
{ }

#endif