using UnityEngine;

public static class BoxColliderExtensions
{
	public static Vector3 GetRandomPoint(this BoxCollider a)
	{
		Vector3 extents = a.bounds.extents;
		
		Vector3 localPoint = new (
			Random.Range(-extents.x, extents.x),
			Random.Range(-extents.y, extents.y),
			Random.Range(-extents.z, extents.z)
		);

		localPoint += a.center;
		return a.transform.TransformPoint(localPoint);
	}
}