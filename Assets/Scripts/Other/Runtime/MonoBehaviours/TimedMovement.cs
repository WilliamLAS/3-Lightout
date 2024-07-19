using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class TimedMovement : MonoBehaviour
{
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

	[Header("TimedMovement Events")]
	#region TimedMovement Events

	[SerializeField]
	private UnityEvent onReachedToDestination;


	#endregion

	// Update
	private void Update()
	{
		UpdateDestinationProgress();
	}

	// TODO: Timer progress used here
	public void MoveToCurrentProgress()
	{
		var timerProgress = (destinationTimer.TickSecond - destinationTimer.CurrentSecond) / destinationTimer.TickSecond;
		controlledRigidbody.position = Vector3.Lerp(startingDestination.position, targetDestination.position, followCurve.Evaluate(timerProgress));
	}

	public void UpdateDestinationProgress()
	{
		if (!destinationTimer.HasEnded)
		{
			MoveToCurrentProgress();
			destinationTimer.Tick();

			if (destinationTimer.HasEnded)
				onReachedToDestination?.Invoke();
		}
	}

	public void ResetDestinationProgress()
	{
		destinationTimer.Reset();
		MoveToCurrentProgress();
	}
}


#if UNITY_EDITOR

public sealed partial class TimedMovement
{ }

#endif