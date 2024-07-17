using UnityEngine;

public sealed partial class Target : MonoBehaviour
{
	[field: SerializeField]
	public TargetType TargetType
	{
		get;
		private set;
	}
}


#if UNITY_EDITOR

public sealed partial class Target
{ }

#endif