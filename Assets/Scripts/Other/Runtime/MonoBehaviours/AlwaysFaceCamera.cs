using UnityEngine;

public sealed partial class AlwaysFaceCamera : MonoBehaviour
{
    public Transform controlled;

	public bool isReversed;

	public Vector3 rotationOffset;

	[VectorRange(minX: -1f, maxX: 1f, minY: -1f, maxY: 1f, minZ: -1f, maxZ: 1f)]
	public Vector3 rotationThreshold = new Vector3(1f, 1f, 1f);


	// Update
	private void Update()
	{
		var lookForward = Vector3.Scale(Camera.main.transform.position - this.transform.position, rotationThreshold);

		if (isReversed)
			controlled.rotation = Quaternion.LookRotation(-lookForward) * Quaternion.Euler(rotationOffset);
		else
			controlled.rotation = Quaternion.LookRotation(lookForward) * Quaternion.Euler(rotationOffset);
	}
}


#if UNITY_EDITOR

public sealed partial class AlwaysFaceCamera
{ }

#endif