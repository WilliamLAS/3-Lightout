using Unity.Cinemachine;
using UnityEngine;

public sealed partial class Level1SceneController : MonoBehaviour
{
	[Header("Level1SceneController Visuals")]
	#region Level1SceneController Visuals

	[SerializeField]
	private CinemachineImpulseSource cameraShaker;


	#endregion


	// Initialize
	private void OnEnable()
	{
		ScreenControllerSingleton.Instance.DoExplosion(2f);
	}

	private void Start()
	{
		cameraShaker.GenerateImpulse();
	}


	// Update
	public void OnFinishedLevel()
	{
		SceneControllerPersistentSingleton.Instance.ChangeActiveSceneToNextLevel();
	}

	public void OnLostLevel()
	{
		SceneControllerPersistentSingleton.Instance.RestartScene();
	}
}


#if UNITY_EDITOR

public sealed partial class Level1SceneController
{ }

#endif