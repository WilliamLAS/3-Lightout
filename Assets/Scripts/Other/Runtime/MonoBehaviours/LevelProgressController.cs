using System;
using UnityEngine;

public sealed partial class LevelProgressController : MonoBehaviour
{
    [SerializeField]
    private Transform floorProgressHider;

    [SerializeField]
    private Vector3 maxFloorProgressHiderScale;

	[SerializeField]
	private float floorProgressHiderSpeed;

	[NonSerialized]
	private float levelProgress;


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
		var updatedLocalScale = Vector3.MoveTowards(floorProgressHider.localScale, maxFloorProgressHiderScale * (1f - levelProgress), floorProgressHiderSpeed * Time.deltaTime);
		floorProgressHider.localScale = updatedLocalScale;
	}
}


#if UNITY_EDITOR

public sealed partial class LevelProgressController
{ }

#endif