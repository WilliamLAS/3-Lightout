using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed partial class ConstantMovement : MonoBehaviour
{
	#region ConstantMovement Movement

	[SerializeField]
    private Rigidbody selfRigidbody;

    [SerializeField]
    private Vector3 velocity;

    [SerializeField]
    [Tooltip("If no limitation wanted for axis, set to zero")]
    private Vector3 maxVelocity;

	[SerializeField]
	private UpdateType updateType;


	#endregion


	// Update
	private void Update()
	{
		if (updateType is UpdateType.Update)
			UpdateVelocity();
	}

	private void FixedUpdate()
	{
		if (updateType is UpdateType.FixedUpdate)
			UpdateVelocity();
	}

	private void LateUpdate()
	{
		if (updateType is UpdateType.LateUpdate)
			UpdateVelocity();
	}

	private void UpdateVelocity()
	{
		selfRigidbody.linearVelocity = velocity;
		LimitVelocity();
	}

	private void LimitVelocity()
	{
		var updatedLinearVelocity = selfRigidbody.linearVelocity;

		if (maxVelocity.x > 0f)
			updatedLinearVelocity.x = Mathf.Clamp(updatedLinearVelocity.x, -maxVelocity.x, maxVelocity.x);

		if (maxVelocity.y > 0f)
			updatedLinearVelocity.y = Mathf.Clamp(updatedLinearVelocity.y, -maxVelocity.y, maxVelocity.y);

		if (maxVelocity.z > 0f)
			updatedLinearVelocity.z = Mathf.Clamp(updatedLinearVelocity.z, -maxVelocity.z, maxVelocity.z);

		selfRigidbody.linearVelocity = updatedLinearVelocity;
	}
}


#if UNITY_EDITOR

public sealed partial class ConstantMovement
{ }

#endif