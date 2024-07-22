using UnityEngine;

public partial class Level3ProgressControllerSingleton : LevelProgressControllerSingletonBase<Level3ProgressControllerSingleton>
{
	[Header("Level3ProgressControllerSingleton Progress")]
	#region Level3ProgressControllerSingleton Progress

	[field: SerializeField]
	public uint PlayerFinishProgressLightAttackerCount
	{ get; private set; }

	[field: SerializeField]
	public float PlayerProgress
	{ get; private set; }

	[field: SerializeField]
	public float StarProgress
	{ get; private set; }


	#endregion

	[Header("Level3ProgressControllerSingleton Visuals")]
	#region Level3ProgressControllerSingleton Visuals

	[SerializeField]
	private Transform floorProgressHider;

	[SerializeField]
	private Vector3 maxFloorProgressHiderScale;

	[SerializeField]
	private float floorProgressHiderSpeed;


	#endregion


	// Update
	protected override void Update()
	{
		UpdateProgress();
		UpdateProgressVisual();
		base.Update();
	}

	protected override void UpdateProgress()
	{
		if (IsProgressFinished)
			return;

		var progress01 = (PlayerProgress + StarProgress) / 2f;
		progress01 = Mathf.Clamp01(progress01);
		_levelProgress = progress01;
	}

	protected override void CheckProgressState()
	{
		if (!IsProgressFinished && _levelProgress >= 0.995f)
		{
			Debug.Log("Progress finished");
			IsProgressFinished = true;
			onProgressFinished?.Invoke();
		}
	}

	private void UpdateProgressVisual()
	{
		var updatedLocalScale = Vector3.MoveTowards(floorProgressHider.localScale, maxFloorProgressHiderScale * (1f - _levelProgress), floorProgressHiderSpeed * Time.deltaTime);
		floorProgressHider.localScale = updatedLocalScale;
	}

	public void UpdatePlayerProgress(uint collectedLightAttackerCount)
	{
		PlayerProgress = (float)collectedLightAttackerCount / PlayerFinishProgressLightAttackerCount;
	}

	public void UpdateStarProgress(float growthScale)
	{
		if (StarProgress >= 1f)
			return;

		StarProgress = growthScale;
	}
}


#if UNITY_EDITOR

public partial class Level3ProgressControllerSingleton
{ }

#endif