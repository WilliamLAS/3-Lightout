using UnityEngine;

public sealed partial class EventReflector : MonoBehaviour
{
    public GameObject reflected;
}


#if UNITY_EDITOR

public sealed partial class EventReflector
{ }

#endif