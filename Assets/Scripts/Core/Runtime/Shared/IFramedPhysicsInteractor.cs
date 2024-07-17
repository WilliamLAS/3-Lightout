using System;

public interface IFrameDependentPhysicsInteractor
{
	/// <remarks> Must not be used in MonoBehaviour.FixedUpdate() </remarks>
	public void DoFrameDependentPhysics();

	/// <summary> Calls all OnXXXExit()'s </summary>
	/// <remarks> In 2D, you dont need to implement this </remarks>
	public void CallExitInteractions();
}

public interface IFrameDependentPhysicsInteractor<InteractionType> : IFrameDependentPhysicsInteractor
	where InteractionType : Enum
{
	public void RegisterFrameDependentPhysicsInteraction(FrameDependentInteraction<InteractionType> interaction);
}