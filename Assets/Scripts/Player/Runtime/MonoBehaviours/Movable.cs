using System;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class Movable : MonoBehaviour
{
	[SerializeField]
	private Rigidbody selfRigidbody;

	[SerializeField]
	private UpdateType updateType;

	[SerializeField]
	private ForceMode movementForceMode;

    public Vector3 movementForce;

	[VectorRange(minX: 0f, minY: 0f, minZ: 0f)]
	[Tooltip("Optional. Set to zero if no velocity limitation wanted")]
	public Vector3 maxMovementVelocity;

	[NonSerialized]
	public Vector3 movingDirection;


	// Initialize


	// Update
	private void Update()
	{
		if (updateType is UpdateType.Update)
		{
			ApplySpeedForceToDirection();
			LimitVelocity();
		}
	}

	private void LateUpdate()
	{
		if (updateType is UpdateType.LateUpdate)
		{
			ApplySpeedForceToDirection();
			LimitVelocity();
		}
	}

	private void FixedUpdate()
	{
		if (updateType is UpdateType.FixedUpdate)
		{
			ApplySpeedForceToDirection();
			LimitVelocity();
		}
	}

	public void ApplySpeedForceToDirection()
	{
		selfRigidbody.AddForce(Vector3.Scale(movementForce, movingDirection.normalized), movementForceMode);
	}

	private void LimitVelocity()
	{
		var updatedLinearVelocity = selfRigidbody.linearVelocity;

		if (maxMovementVelocity.x > 0f)
			updatedLinearVelocity.x = Mathf.Clamp(updatedLinearVelocity.x, -maxMovementVelocity.x, maxMovementVelocity.x);

		if (maxMovementVelocity.y > 0f)
			updatedLinearVelocity.y = Mathf.Clamp(updatedLinearVelocity.y, -maxMovementVelocity.y, maxMovementVelocity.y);

		if (maxMovementVelocity.z > 0f)
			updatedLinearVelocity.z = Mathf.Clamp(updatedLinearVelocity.z, -maxMovementVelocity.z, maxMovementVelocity.z);

		selfRigidbody.linearVelocity = updatedLinearVelocity;
	}

	public void SetMovingDirection(CallbackContext context)
	{
		var inputValue = context.ReadValue<Vector2>();
		movingDirection = new Vector3(inputValue.x, 0f, inputValue.y);
	}


	// Dispose
}


#if UNITY_EDITOR

public sealed partial class Movable
{ }

#endif