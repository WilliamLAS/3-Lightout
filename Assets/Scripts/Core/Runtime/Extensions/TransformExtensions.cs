using System;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
	public static bool TryGetNearestTransform<TransformEnumeratorType>(this Transform relativeTo, TransformEnumeratorType transformEnumerator, out Transform nearestTransform, Predicate<Transform> predicateNearest = null)
		where TransformEnumeratorType : IEnumerator<Transform>
	{
		nearestTransform = default;

		var isFoundNearest = false;
		float nearestHorizontalDistance = float.MaxValue;
		float iteratedDistance;

		// Check sqr distances and select nearest
		foreach (var iteratedTransform in transformEnumerator)
		{
			iteratedDistance = (iteratedTransform.position - relativeTo.position).sqrMagnitude;

			if ((iteratedDistance < nearestHorizontalDistance) && (predicateNearest == null || predicateNearest.Invoke(iteratedTransform)))
			{
				nearestTransform = iteratedTransform;
				nearestHorizontalDistance = iteratedDistance;
				isFoundNearest = true;
			}
		}

		return isFoundNearest;
	}
}
