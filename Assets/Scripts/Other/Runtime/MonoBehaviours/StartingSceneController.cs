using FMODUnity;
using UnityEngine;

public sealed partial class StartingSceneController : MonoBehaviour
{
	private enum SceneStage
	{
		None,
		FallingLight,
		Explosion,
		PlayerCreation
	}

	[Header("StartingSceneController Stage")]
	#region StartingSceneController Stage

	[SerializeField]
	private SceneStage _stage;

	private SceneStage Stage
	{
		get => _stage;
		set
		{
			if (value != _stage)
			{
				_stage = value;
				OnStageChanged(value);
			}
		}
	}


	#endregion

	[Header("StartingSceneController Falling Light")]
	#region StartingSceneController Falling Light

	[SerializeField]
	private Timer endFallingLightStageTimer;


	#endregion

	[Header("StartingSceneController Explosion")]
	#region StartingSceneController Explosion

	[SerializeField]
	private ParticleSystem explosionPS;

	[SerializeField]
	private Timer endExplosionStageTimer;

	[SerializeField]
	private ParticleSystemForceField explosionPSForceField;


	#endregion

	[Header("StartingSceneController Player Creation")]
	#region StartingSceneController Player Creation



	#endregion

	[Header("StartingSceneController Sounds")]
	#region StartingSceneController Sounds

	[SerializeField]
    private StudioEventEmitter ambienceEmitter;

	[SerializeField]
	private StudioEventEmitter explosionEmitter;

	[SerializeField]
	private StudioEventEmitter playerCreationEmitter;


	#endregion


	// Initialize
	private void OnEnable()
	{
		ScreenControllerSingleton.Instance.DoFade(0f, 2f, 1f);
		Stage = SceneStage.FallingLight;
	}


	// Update
	private void Update()
	{
		if (!ambienceEmitter.IsPlaying())
			ambienceEmitter.Play();

		DoStage();
	}

	private void DoStage()
	{
		switch (_stage)
		{
			case SceneStage.FallingLight:
			DoFallingLightStage();
			break;

			case SceneStage.Explosion:
			DoExplosionStage();
			break;

			case SceneStage.PlayerCreation:
			DoPlayerCreationStage();
			break;
		}
	}

	private void DoFallingLightStage()
	{
		if (!endFallingLightStageTimer.HasEnded && endFallingLightStageTimer.Tick())
			Stage = SceneStage.Explosion;
	}

	private void DoExplosionStage()
	{
		if (!endExplosionStageTimer.HasEnded && endExplosionStageTimer.Tick())
		{
			explosionPSForceField.gameObject.SetActive(true);
			Stage = SceneStage.PlayerCreation;
		}
	}

	private void DoPlayerCreationStage()
	{ }

	private void OnStageChanged(SceneStage value)
	{
		switch (_stage)
		{
			case SceneStage.FallingLight:
			OnStageChangedToFallingLight();
			break;

			case SceneStage.Explosion:
			OnStageChangedToExplosion();
			break;

			case SceneStage.PlayerCreation:
			OnStageChangedToPlayerCreation();
			break;
		}
	}

	private void OnStageChangedToFallingLight()
	{
		ambienceEmitter.Play();
	}

	private void OnStageChangedToExplosion()
	{
		explosionEmitter.Play();
		explosionPS.Play();
		ScreenControllerSingleton.Instance.DoExplosion(5f);
	}

	private void OnStageChangedToPlayerCreation()
	{
		OnFinishedLevel();
	}

	public void OnFinishedLevel()
	{
		playerCreationEmitter.Stop();
		playerCreationEmitter.Play();
		ScreenControllerSingleton.Instance.DoFade(1f, 2f, onFadeEnded:
				() => SceneControllerPersistentSingleton.Instance.ChangeActiveSceneTo(Scenes.Level1));
	}
}


#if UNITY_EDITOR

public sealed partial class StartingSceneController
{ }

#endif