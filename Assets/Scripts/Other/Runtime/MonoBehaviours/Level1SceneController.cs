using Unity.Cinemachine;
using UnityEngine;

public partial class Level1SceneController : MonoBehaviour
{
	[Header("Level1SceneController Visuals")]
	#region Level1SceneController Visuals

	[SerializeField]
	protected CinemachineImpulseSource enteredLevelCameraShaker;


	#endregion


	// Initialize
	protected virtual void OnEnable()
	{
		ScreenControllerSingleton.Instance.DoExplosion(2f);
	}

	protected virtual void Start()
	{
		enteredLevelCameraShaker.GenerateImpulse();
	}


	// Update
	public virtual void OnFinishedLevel()
	{
		SceneControllerPersistentSingleton.Instance.ChangeActiveSceneToNextLevel();
	}

	public virtual void OnLostLevel()
	{
		SceneControllerPersistentSingleton.Instance.RestartScene();
	}
}


#if UNITY_EDITOR

public partial class Level1SceneController
{ }

#endif