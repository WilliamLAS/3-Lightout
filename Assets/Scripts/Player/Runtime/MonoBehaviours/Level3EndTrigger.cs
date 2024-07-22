using System.Collections.Generic;
using UnityEngine;

public sealed partial class Level3EndTrigger : MonoBehaviour, IFrameDependentPhysicsInteractor<Level3EndTrigger.PhysicsInteraction>
{
	public enum PhysicsInteraction
	{
		OnEndTriggerEnter,
	}

	#region Enemy Other

	private Queue<FrameDependentInteraction<PhysicsInteraction>> FrameDependentInteractionQueue
	{ get; } = new();


	#endregion


	// Update
	private void Update()
	{
		DoFrameDependentPhysics();
	}

	public void RegisterFrameDependentPhysicsInteraction(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!FrameDependentInteractionQueue.Contains(interaction))
			FrameDependentInteractionQueue.Enqueue(interaction);
	}

	public void DoFrameDependentPhysics()
	{
		for (int i = FrameDependentInteractionQueue.Count; i > 0; i--)
		{
			var iteratedPhysicsInteraction = FrameDependentInteractionQueue.Dequeue();

			switch (iteratedPhysicsInteraction.interactionType)
			{
				case PhysicsInteraction.OnEndTriggerEnter:
				DoEndTriggerEnter(iteratedPhysicsInteraction);
				break;
			}
		}
	}

	private void DoEndTriggerEnter(FrameDependentInteraction<PhysicsInteraction> interaction)
	{
		if (!interaction.collider)
			return;

		if (EventReflectorUtils.TryGetComponentByEventReflector<Player>(interaction.collider.gameObject, out _))
			GameControllerPersistentSingleton.Instance.FinishedLevel();
	}


	public void OnEndTriggerEnter(Collider other)
		=> RegisterFrameDependentPhysicsInteraction(new(PhysicsInteraction.OnEndTriggerEnter, other, null));


	// Dispose
	public void CallExitInteractions()
	{
		throw new System.NotImplementedException();
	}
}


#if UNITY_EDITOR

public sealed partial class Level3EndTrigger
{ }

#endif