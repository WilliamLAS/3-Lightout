using System;
using UnityEngine;

public sealed partial class LevelProgressController : MonoBehaviour
{
    [SerializeField]
    private Transform floorProgressHider;

    [NonSerialized]
    private Vector3 initialFloorProgressHiderScale;

	[SerializeField]
	private float floorProgressHiderSpeed;

	[NonSerialized]
	private float levelProgress;


	// Initialize
	private void Start()
	{
		initialFloorProgressHiderScale = floorProgressHider.localScale;
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

		updatedLocalScale = Vector3.MoveTowards(floorProgressHider.localScale, initialFloorProgressHiderScale * (1f - levelProgress), floorProgressHiderSpeed * Time.deltaTime);
		updatedLocalScale.z = floorProgressHider.localScale.z;
		floorProgressHider.localScale = updatedLocalScale;
	}
}


#if UNITY_EDITOR

public sealed partial class LevelProgressController
{ }

#endif