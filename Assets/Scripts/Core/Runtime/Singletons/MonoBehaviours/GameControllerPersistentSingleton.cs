using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public sealed partial class GameControllerPersistentSingleton : MonoBehaviourSingletonBase<GameControllerPersistentSingleton>
{
	[Header("GameControllerPersistentSingleton Events")]
	#region GameControllerPersistentSingleton Events

	[SerializeField]
	private UnityEvent onRestartGame = new();

	[SerializeField]
	private UnityEvent onLostGame = new();

	[SerializeField]
	private UnityEvent onFinishedLevel = new();


	#endregion

	#region GameControllerPersistentSingleton States

	[NonSerialized]
	private static float lastTimeScaleBeforePause = 1f;

	[field: NonSerialized]
	public static JSVisibilityStateType VisibilityState { get; private set; }

	[field: NonSerialized]
	public static bool IsQuitting { get; private set; }

	public static bool IsPaused => (Time.timeScale == 0);


	#endregion


	// Initialize
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
	private static void OnBeforeSplashScreen()
	{
		SceneManager.activeSceneChanged += OnActiveSceneChanged;
	}


	// Update
	public void PauseGame()
	{
		lastTimeScaleBeforePause = Time.timeScale;
		Time.timeScale = 0;
	}

	public void ResumeGame()
	{
		if (IsPaused)
			Time.timeScale = lastTimeScaleBeforePause;
	}

	public void RestartGame()
	{
		onRestartGame?.Invoke();
		ResumeGame();
		SceneControllerPersistentSingleton.Instance.RestartScene();
	}

	public void FinishedLevel()
	{
		SaveDataControllerSingleton.Instance.SaveDataToFile();
		onFinishedLevel?.Invoke();
	}

	public void LostGame()
	{
		PauseGame();
		onLostGame?.Invoke();
	}


	private static void OnActiveSceneChanged(Scene lastScene, Scene loadedScene)
	{
		if (!IsAnyInstanceLiving)
			CreateSingleton();
	}

	private void OnVisibilityChange(string value) => VisibilityState = Enum.Parse<JSVisibilityStateType>(value, true);

	private void OnBeforeUnload() => IsQuitting = true;

	// TODO: In mobile, this should act like OnBeforeUnload. See: https://www.igvita.com/2015/11/20/dont-lose-user-and-app-state-use-page-visibility/
	private void OnPageHide(int isPersisted) => IsQuitting = true;
}


#if UNITY_EDITOR

public sealed partial class GameControllerPersistentSingleton
{
	private void OnApplicationPause(bool pause)
	{
		if (pause)
			VisibilityState = JSVisibilityStateType.Hidden;
		else
			VisibilityState = JSVisibilityStateType.Visible;
	}

	private void OnApplicationQuit()
	{
		IsQuitting = true;
	}
}

#endif