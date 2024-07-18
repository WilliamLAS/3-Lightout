using UnityEngine;
using UnityEngine.Events;

public sealed partial class Player : MonoBehaviour
{
    [Header("Player Light Attacker")]
    #region Player Light Attacker

    [SerializeField]
    private uint _collectedLightAttackerCount;

    [SerializeField]
    private uint _maxCollectableLightAttackerCount;

    public uint CollectedLightAttackerCount => _collectedLightAttackerCount;

	public uint MaxCollectableLightAttackerCount => _maxCollectableLightAttackerCount;


	#endregion

	[Header("Player Events")]
	#region Player Events

	[SerializeField]
	private UnityEvent<float> onLevelProgressChanged = new();



	#endregion


	// Update
	public void UpdateLevelProgress()
    {
        onLevelProgressChanged?.Invoke((float)_collectedLightAttackerCount / _maxCollectableLightAttackerCount);
	}

	public void OnKilledOtherEnemy(Enemy killed)
	{
		SceneControllerPersistentSingleton.Instance.RestartScene();
	}

	public void OnGotKilledByEnemy(Enemy killedBy)
    {
        //GameControllerPersistentSingleton.Instance.LostGame();
        SceneControllerPersistentSingleton.Instance.RestartScene();
    }

    public void OnGrabbedLightAttacker(LightAttacker lightAttacker)
    {
        _collectedLightAttackerCount++;
        UpdateLevelProgress();

		if (_collectedLightAttackerCount >= _maxCollectableLightAttackerCount)
            GameControllerPersistentSingleton.Instance.FinishedLevel();
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

			if (_collectedLightAttackerCount == 0)
            {
                //GameControllerPersistentSingleton.Instance.LostGame();
                Debug.Log("Lost Game");
            }
        }
	}
}


#if UNITY_EDITOR

public sealed partial class Player
{ }

#endif