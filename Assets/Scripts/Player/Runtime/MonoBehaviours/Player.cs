using FMODUnity;
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

    public bool IsFinishedLevel
    { get; private set; }


	#endregion

	[Header("Player Enemy")]
	#region Player Enemy

	[SerializeField]
	private Enemy enemyController;


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


	// Initialize
	private void OnEnable()
	{
        idleEmitter.Play();
	}


	// Update
	public void UpdateLevelProgress()
    {
        onLevelProgressChanged?.Invoke((float)_collectedLightAttackerCount / _finishLevelLightAttackerCount);
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
            _collectedLightAttackerCount = 0;
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