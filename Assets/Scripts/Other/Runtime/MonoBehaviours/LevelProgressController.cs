using System;
using UnityEngine;

public sealed partial class LevelProgressController : MonoBehaviour
{
    [SerializeField]
    private Transform floorProgressHider;

    [NonSerialized]
    private float initialFloorProgressHiderScaleX;

	[SerializeField]
	private float floorProgressHiderSpeed;

	[NonSerialized]
	private float levelProgress;


	// Initialize
	private void Start()
	{
		initialFloorProgressHiderScaleX = floorProgressHider.localScale.x;
	}


	// Update
	private void Update()
	{
		UpdateLevelProgressVisual();
	}

	public void UpdateLevelProgress(float progress01)
	{
		progress01 = Mathf.Clamp01(progress01);
		levelProgress = progress01;
	}

	private void UpdateLevelProgressVisual()
	{
		var updatedLocalScale = floorProgressHider.localScale;

		updatedLocalScale.x = Mathf.MoveTowards(floorProgressHider.localScale.x, initialFloorProgressHiderScaleX * (1f - levelProgress), floorProgressHiderSpeed * Time.deltaTime);
		floorProgressHider.localScale = updatedLocalScale;
	}
}


#if UNITY_EDITOR

public sealed partial class LevelProgressController
{ }

#endif