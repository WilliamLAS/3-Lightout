using System;
using System.Collections.Generic;
using UnityEngine;

public readonly struct FrameDependentInteraction<InteractionType> : IEquatable<FrameDependentInteraction<InteractionType>>
	where InteractionType : Enum
{
	public readonly InteractionType interactionType;

	public readonly Collider collider;

	public readonly Collision collision;


	public FrameDependentInteraction(InteractionType interactionType, Collider collider, Collision collision)
	{
		this.interactionType = interactionType;
		this.collider = collider;
		this.collision = collision;
	}

	public override bool Equals(object obj)
	{
		return (obj is FrameDependentInteraction<InteractionType> interaction) && Equals(interaction);
	}

	public bool Equals(FrameDependentInteraction<InteractionType> other)
	{
		return EqualityComparer<InteractionType>.Default.Equals(interactionType, other.interactionType) &&
			   EqualityComparer<Collider>.Default.Equals(collider, other.collider) &&
			   EqualityComparer<Collision>.Default.Equals(collision, other.collision);
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(interactionType, collider, collision);
	}

	public static bool operator ==(FrameDependentInteraction<InteractionType> left, FrameDependentInteraction<InteractionType> right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FrameDependentInteraction<InteractionType> left, FrameDependentInteraction<InteractionType> right)
	{
		return !(left == right);
	}
}