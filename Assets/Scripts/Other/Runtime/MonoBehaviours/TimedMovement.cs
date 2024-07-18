using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class TimedMovement : MonoBehaviour
{
	[Header("TimedMovement Destination")]
	#region

	public bool stopMovement;


	#endregion

	[Header("TimedMovement Destination")]
	#region TimedMovement Destination

	[SerializeField]
	private Rigidbody controlledRigidbody;

	[SerializeField]
    private Timer destinationTimer;

	[SerializeField]
	private AnimationCurve followCurve;

	[SerializeField]
	private Transform startingDestination;

	[SerializeField]
	private Transform targetDestination;


	#endregion

	// Update
	private void Update()
	{
		// TODO: Timer progress used here
		if (!stopMovement && !destinationTimer.Tick())
		{
			var timerProgress = (destinationTimer.TickSecond - destinationTimer.CurrentSecond) / destinationTimer.TickSecond;
			controlledRigidbody.position = Vector3.Lerp(startingDestination.position, targetDestination.position, followCurve.Evaluate(timerProgress));
		}
	}
}


#if UNITY_EDITOR

public sealed partial class TimedMovement
{ }

#endif