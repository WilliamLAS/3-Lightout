using UnityEngine;
using UnityEngine.Events;

public abstract partial class EventBase<EventType> : MonoBehaviour
    where EventType : UnityEventBase
{
    [SerializeField]
    protected EventType onRaised;
}


#if UNITY_EDITOR

public abstract partial class EventBase
{ }

#endif