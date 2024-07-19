using System;
using UnityEngine;

public sealed partial class NegatedLight : MonoBehaviour
{
	[SerializeField]
    private Light controlled;

	[NonSerialized]
	private Color lastColor;

	[NonSerialized]
	private Color negatedColor;


	// Initialize
	private void Awake()
	{
		UpdateColor();
	}

	private void OnEnable()
	{
		lastColor = controlled.color;
	}

	// Update
	private void Update()
	{
		UpdateColor();
	}

	private void UpdateColor()
	{
		if (controlled.color != negatedColor)
		{
			lastColor = controlled.color;
			negatedColor = controlled.color * -1f;
			controlled.color = negatedColor;
		}
	}


	// Dispose
	private void OnDisable()
	{
		controlled.color = lastColor;
	}
}


#if UNITY_EDITOR

public sealed partial class NegatedLight
{ }

#endif