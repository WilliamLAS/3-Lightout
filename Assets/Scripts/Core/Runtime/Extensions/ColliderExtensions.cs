using UnityEngine;

public static class ColliderExtensions
{
	public static Vector3 GetRandomPoint(this Collider a)
	{
        switch (a)
		{
			case BoxCollider:
			return (a as BoxCollider).GetRandomPoint();

			case SphereCollider:
			return (a as SphereCollider).GetRandomPoint();
		}

		Debug.LogErrorFormat("Type {0} is un-supported. BoxCollider and SphereCollider is supported only", a.GetType());
		return default;
	}
}