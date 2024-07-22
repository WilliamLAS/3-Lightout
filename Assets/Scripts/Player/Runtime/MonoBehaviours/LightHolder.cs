using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public sealed partial class LightHolder : MonoBehaviour
{
	[Header("LightHolder Progress")]
	#region LightHolder Progress

	[SerializeField]
	private Timer lightOutTimer;

	[NonSerialized]
	private float _lightOutProgress;

	public float LightOutProgress => _lightOutProgress;

	public bool IsLightening => !lightOutTimer.HasEnded;


	#endregion

	[Header("LightHolder Enemy")]
	#region LightHolder Enemy

	[SerializeField]
	private Enemy enemyController;


	#endregion

	[Header("LightHolder Visual")]
	#region LightHolder Visual

	[SerializeField]
	private Transform selfVisual;

	[SerializeField]
	private Vector3 minVisualScale;

	[SerializeField]
	private Vector3 maxVisualScale;

	[SerializeField]
	private Light selfLight;

	[SerializeField]
	private float minLightIntensity;

	[SerializeField]
	private float maxLightIntensity;

	[SerializeField]
	private ParticleSystem[] onLightenPSEffects;

	[SerializeField]
	private CinemachineImpulseSource onLightenCameraShaker;


	#endregion

	#region LightHolder Events

	[SerializeField]
	private UnityEvent onLighten = new();

	[SerializeField]
	private UnityEvent onLightOut = new();

	#endregion


	// Initialize
	private void Start()
	{
		UpdateLightByProgress();
		UpdateScaleByProgress();
		RestartLightening();
	}


	// Update
	private void Update()
	{
		// TODO: Uses timer progress
		if (!lightOutTimer.HasEnded)
		{
			lightOutTimer.Tick();
			_lightOutProgress = (lightOutTimer.CurrentSecond / lightOutTimer.TickSecond);

			if (lightOutTimer.HasEnded)
				LightOut();

			UpdateLightByProgress();
			UpdateScaleByProgress();
		}
	}

	private void LightOut()
	{
		foreach (var iteratedPS in onLightenPSEffects)
			iteratedPS.Stop();

		enemyController.enabled = true;
		lightOutTimer.Finish();
		onLightOut?.Invoke();
	}

	public void RestartLightening()
	{
        foreach (var iteratedPS in onLightenPSEffects)
			iteratedPS.Play();

		enemyController.enabled = false;
		onLightenCameraShaker.GenerateImpulse();
        lightOutTimer.Reset();
		onLighten?.Invoke();
	}

	public void UpdateLightByProgress()
	{
		selfLight.intensity = Mathf.Lerp(minLightIntensity, maxLightIntensity, _lightOutProgress);
	}

	private void UpdateScaleByProgress()
	{
		selfVisual.localScale = Vector3.Lerp(minVisualScale, maxVisualScale, _lightOutProgress);
	}


	// Dispose
}


#if UNITY_EDITOR

public sealed partial class LightHolder
{ }

#endif