using FMODUnity;
using System;
using UnityEngine;
using UnityEngine.Events;

public sealed partial class Player : MonoBehaviour
{
    [Header("Player Light Attacker")]
    #region Player Light Attacker

    [SerializeField]
    private uint _collectedLightAttackerCount;

    [SerializeField]
    private uint _finishLevelLightAttackerCount;

    public uint CollectedLightAttackerCount => _collectedLightAttackerCount;

	public uint FinishLevelLightAttackerCount => _finishLevelLightAttackerCount;


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
	private Vector3 finishedLevelMaxScale;


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
	private UnityEvent<float> onLevelProgressChanged = new();



	#endregion

	#region Player Other

	public bool IsFinishedLevel
	{ get; private set; }

	[NonSerialized]
	private float levelProgress;


	#endregion


	// Initialize
	private void OnEnable()
	{
		_collectedLightAttackerCount = 1;
		UpdateLevelProgress();
		UpdateLevelProgressVisual();
		idleEmitter.Play();
	}


	// Update
	private void Update()
	{
		UpdateLevelProgressVisual();
	}

	public void UpdateLevelProgress()
    {
		levelProgress = (float)_collectedLightAttackerCount / _finishLevelLightAttackerCount;
		onLevelProgressChanged?.Invoke(levelProgress);
	}

	private void UpdateLevelProgressVisual()
	{
		var oldLocalScale = visual.localScale;
		var updatedLocalScale = Vector3.MoveTowards(oldLocalScale, finishedLevelMaxScale * levelProgress, scaleSpeed * Time.deltaTime);

		visual.localScale = updatedLocalScale;
	}

	public void OnKilledOtherEnemy(Enemy killed)
	{ }

	public void OnGotKilledByEnemy(Enemy killedBy)
    {
		GameControllerPersistentSingleton.Instance.LostGame();
    }

    public void OnGrabbedLightAttacker(LightAttacker lightAttacker)
    {
        lightAttackerCountChanged.Play();
		_collectedLightAttackerCount++;
        UpdateLevelProgress();

		if (!enemyController.IsDead && !IsFinishedLevel && (_collectedLightAttackerCount >= _finishLevelLightAttackerCount))
        {
            IsFinishedLevel = true;
            GameControllerPersistentSingleton.Instance.FinishedLevel();
		}
    }

    public void OnUnGrabbedLightAttacker(LightAttacker lightAttacker)
    {
		try
        {
            _collectedLightAttackerCount = checked(_collectedLightAttackerCount - 1);
        }

        catch
        {
            _collectedLightAttackerCount = 1;
        }
        finally
        {
			UpdateLevelProgress();
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