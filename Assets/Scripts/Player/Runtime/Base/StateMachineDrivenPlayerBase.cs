using UnityEngine;

public abstract partial class StateMachineDrivenPlayerBase : MonoBehaviour
{
    [SerializeField]
    private PlayerStateType _state;

    public PlayerStateType State
    {
        get => _state;
        set
        {
            if (value != _state)
            {
                _state = value;
				OnStateChangedToAny(value);
                OnStateChanged();
            }
        }
    }


	// Initialize
	protected virtual void OnEnable()
	{
        State = PlayerStateType.Idle;
	}


	// Update
	protected virtual void Update()
	{
        DoState();
	}

	private void DoState()
	{
		switch (_state)
        {
			case PlayerStateType.Idle:
			DoIdleState();
			break;

			case PlayerStateType.Following:
			DoFollowingState();
			break;

			case PlayerStateType.Attacking:
			DoAttackingState();
			break;

			case PlayerStateType.Dead:
			DoDeadState();
			break;
		}
	}

	protected virtual void DoIdleState()
	{ }

	protected virtual void DoFollowingState()
	{ }

	protected virtual void DoAttackingState()
	{ }

	protected virtual void DoDeadState()
	{ }

	private void OnStateChanged()
	{
		switch (_state)
        {
            case PlayerStateType.Idle:
            OnStateChangedToIdle();
            break;

			case PlayerStateType.Following:
			OnStateChangedToFollowing();
			break;

			case PlayerStateType.Attacking:
            OnStateChangedToAttacking();
            break;

			case PlayerStateType.Dead:
			OnStateChangedToDead();
			break;
		}
	}

	protected virtual void OnStateChangedToIdle()
	{ }

	protected virtual void OnStateChangedToFollowing()
	{ }

	protected virtual void OnStateChangedToAttacking()
	{ }

	protected virtual void OnStateChangedToDead()
	{ }

	/// <summary> Called before other changed calls </summary>
	protected virtual void OnStateChangedToAny(PlayerStateType newState)
	{ }
}


#if UNITY_EDITOR

public abstract partial class StateMachineDrivenPlayerBase
{ }

#endif