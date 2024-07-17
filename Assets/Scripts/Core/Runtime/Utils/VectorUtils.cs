using System;
using UnityEngine;

public static class VectorUtils
{
	private static readonly System.Random randomizer = new();

	public static Vector3 RandomRange(Vector3 minInclusive, Vector3 maxExclusive)
	{
		var x = randomizer.NextFloat(minInclusive.x, maxExclusive.x);
		var y = randomizer.NextFloat(minInclusive.y, maxExclusive.y);
		var z = randomizer.NextFloat(minInclusive.z, maxExclusive.z);
		return new Vector3(x, y, z);
	}

	public static Vector2 RadianToNormalizedVector(float angleInRadians)
	{
		return new Vector2(MathF.Cos(angleInRadians), MathF.Sin(angleInRadians));
	}

	public static Vector3 RadianToVector(float angleInRadians, Vector3 axisToRotateAround)
		=> DegreeToVector(angleInRadians * Mathf.Rad2Deg, axisToRotateAround);

	public static Vector2 DegreeToNormalizedVector(float angleInDegrees)
		=> RadianToNormalizedVector(angleInDegrees * Mathf.Deg2Rad);

	public static Vector3 DegreeToVector(float angleInDegrees, Vector3 axisToRotateAround)
		=> Quaternion.AngleAxis(angleInDegrees, axisToRotateAround).eulerAngles;
}
