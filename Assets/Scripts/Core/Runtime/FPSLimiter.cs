using UnityEngine;

public sealed partial class FPSLimiter : MonoBehaviour
{
    public int limit = 144;


	// Update
	private void Update()
	{
		if (Application.targetFrameRate != limit)
			Application.targetFrameRate = limit;
	}
}


#if UNITY_EDITOR

public sealed partial class FPSLimiter
{ }

#endif