using UnityEngine;

public static class SphereColliderExtensions
{
	public static Vector3 GetRandomPoint(this SphereCollider a)
	{
		Vector3 localPoint = Random.insideUnitSphere * a.radius;
		localPoint += a.center;
		return a.transform.TransformPoint(localPoint);
	}
}