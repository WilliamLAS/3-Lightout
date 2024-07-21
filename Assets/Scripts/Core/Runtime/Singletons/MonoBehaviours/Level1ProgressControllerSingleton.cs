using UnityEngine;

public partial class Level1ProgressControllerSingleton : LevelProgressControllerSingletonBase<Level1ProgressControllerSingleton>
{
	[Header("Level1ProgressControllerSingleton Progress")]
	#region Level1ProgressControllerSingleton Progress

	[field: SerializeField]
	public uint PlayerFinishProgressLightAttackerCount
	{ get; private set; }

	public float PlayerProgress
	{ get; private set; }


	#endregion

	[Header("Level1ProgressControllerSingleton Visuals")]
	#region Level1ProgressControllerSingleton Visuals

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
		var progress01 = PlayerProgress;
		progress01 = Mathf.Clamp01(progress01);
		_levelProgress = progress01;
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
}


#if UNITY_EDITOR

public partial class Level1ProgressControllerSingleton
{ }

#endif