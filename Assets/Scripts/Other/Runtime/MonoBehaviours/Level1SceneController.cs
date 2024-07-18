using UnityEngine;

public sealed partial class Level1SceneController : MonoBehaviour
{
	// Initialize
	private void OnEnable()
	{
		ScreenControllerSingleton.Instance.DoFade(0f, 2f, 1f);
	}


	// Update
	public void OnFinishedLevel()
	{
		ScreenControllerSingleton.Instance.DoExplosion(2f,
			onExplosionEnded: () => ScreenControllerSingleton.Instance.DoFade(1f, 2f,
			onFadeEnded: () => SceneControllerPersistentSingleton.Instance.ChangeActiveSceneToNextLevel()));
		;
	}
}


#if UNITY_EDITOR

public sealed partial class Level1SceneController
{ }

#endif