using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public sealed partial class Player : MonoBehaviour
{
    [Header("Player Light Attacker")]
    #region Player Light Attacker

    [NonSerialized]
    private uint _collectedLightAttackerCount;

    public uint CollectedLightAttackerCount
	{
		get => _collectedLightAttackerCount;
		set
		{
			if (value != _collectedLightAttackerCount)
			{
				_collectedLightAttackerCount = value;
				onLightAttackerCountChanged?.Invoke(value);
			}
		}
	}


	#endregion

	[Header("Player Enemy")]
	#region Player Enemy

	[SerializeField]
	private Enemy enemyController;


	#endregion

	[Header("Player Visuals")]
	#region Player Visuals

	[SerializeField]
	private Transform visual;

	[SerializeField]
	private float scaleSpeed;

	[SerializeField]
	private Vector3 maxScaleWhenCollectedLightAttackerCount;


	#endregion

	[Header("Player Sounds")]
    #region Player Sounds

    [SerializeField]
    private StudioEventEmitter idleEmitter;

	[SerializeField]
	private StudioEventEmitter lightAttackerCountChanged;


	#endregion

	[Header("Player Events")]
	#region Player Events

	[SerializeField]
	private UnityEvent<uint> onLightAttackerCountChanged = new();



	#endregion


	// Initialize
	private void OnEnable()
	{
		_collectedLightAttackerCount = 1;
		onLightAttackerCountChanged?.Invoke(_collectedLightAttackerCount);
		UpdateVisualScaleIfSceneValid();
		idleEmitter.Play();
	}


	// Update
	private void Update()
	{
		UpdateVisualScaleIfSceneValid();
	}

	private void UpdateVisualScaleIfSceneValid()
	{
		var oldLocalScale = visual.localScale;
		var updatedLocalScale = oldLocalScale;

		switch (SceneManager.GetActiveScene().name)
		{
			case Scenes.Level1:
			updatedLocalScale = Vector3.MoveTowards(oldLocalScale, maxScaleWhenCollectedLightAttackerCount * Level1ProgressControllerSingleton.Instance.PlayerProgress, scaleSpeed * Time.deltaTime);
			break;

			case Scenes.Level2:
			updatedLocalScale = Vector3.MoveTowards(oldLocalScale, maxScaleWhenCollectedLightAttackerCount * Level2ProgressControllerSingleton.Instance.PlayerProgress, scaleSpeed * Time.deltaTime);
			break;
		}

		visual.localScale = updatedLocalScale;
	}

	public void OnKilledOtherEnemy(Enemy killed)
	{ }

	public void OnGotKilledByEnemy(Enemy killedBy)
    {
		GameControllerPersistentSingleton.Instance.LostGame();
    }

    public void OnGrabbedCarryable(Carryable carryable)
    {
		var isLightAttacker = EventReflectorUtils.TryGetComponentByEventReflector<LightAttacker>(carryable.gameObject, out _);

		if (!isLightAttacker)
			return;

		lightAttackerCountChanged.Play();
		CollectedLightAttackerCount++;
    }

    public void OnUngrabbedCarryable(Carryable carryable)
    {
		var isLightAttacker = EventReflectorUtils.TryGetComponentByEventReflector<LightAttacker>(carryable.gameObject, out _);

		if (!isLightAttacker)
			return;

		try
        {
			CollectedLightAttackerCount = checked(_collectedLightAttackerCount - 1);
        }

        catch
        {
			CollectedLightAttackerCount = 1;
        }
	}


	// Dispose
	private void OnDisable()
	{
		idleEmitter.Stop();
	}
}


#if UNITY_EDITOR

public sealed partial class Player
{ }

#endif