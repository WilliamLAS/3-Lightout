using UnityEngine;

public sealed partial class Level3Star : MonoBehaviour
{
	[Header("Level3Star Progress")]
	#region Level3Star Progress

	[SerializeField]
	private float growSpeed;

	[SerializeField]
	private LightHolder lightHolder1;

	[SerializeField]
	private LightHolder lightHolder2;

	[SerializeField]
	private LightHolder lightHolder3;

	[SerializeField]
	private LightHolder lightHolder4;

	public bool IsAbleToContinueGrowing => lightHolder1.IsLightening && lightHolder2.IsLightening && lightHolder3.IsLightening && lightHolder4.IsLightening;


	#endregion

	[Header("Level3Star Visuals")]
	#region Level3Star Visuals

	[SerializeField]
	private Transform selfVisual;

	[SerializeField]
	private Vector3 minVisualScale;

	[SerializeField]
    private Vector3 maxVisualScale;


	#endregion


	// Update
	private void Update()
	{
		UpdateProgress();
		UpdateScaleByProgress();
	}

	private void UpdateProgress()
	{
		float newProgress;

		if (IsAbleToContinueGrowing)
			newProgress = Mathf.MoveTowards(Level3ProgressControllerSingleton.Instance.StarProgress, 1f, growSpeed * Time.deltaTime);
		else
			newProgress = Mathf.MoveTowards(Level3ProgressControllerSingleton.Instance.StarProgress, 0f, growSpeed * Time.deltaTime);

		Level3ProgressControllerSingleton.Instance.UpdateStarProgress(newProgress);
	}

	private void UpdateScaleByProgress()
	{
		selfVisual.localScale = selfVisual.localScale = Vector3.Lerp(minVisualScale, maxVisualScale, Level3ProgressControllerSingleton.Instance.StarProgress);
	}
}


#if UNITY_EDITOR

public sealed partial class Level3Star
{ }

#endif
