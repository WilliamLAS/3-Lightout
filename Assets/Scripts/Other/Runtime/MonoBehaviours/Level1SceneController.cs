using UnityEngine;

public sealed partial class Level1SceneController : MonoBehaviour
{
	// Initialize
	private void OnEnable()
	{
		ScreenControllerSingleton.Instance.DoExplosion(2f);
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