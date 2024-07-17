using UnityEngine;

public sealed partial class Player : MonoBehaviour
{
    [SerializeField]
    private uint collectedLightAttackerCount;

    [SerializeField]
    private uint maxCollectableLightAttackerCount;


	// Update
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
        collectedLightAttackerCount++;

        /*if (collectedLightAttackerCount >= maxCollectableLightAttackerCount)
            GameControllerPersistentSingleton.Instance.FinishedLevel();*/
	}

    public void OnUnGrabbedLightAttacker(LightAttacker lightAttacker)
    {
        try
        {
            collectedLightAttackerCount = checked(collectedLightAttackerCount - 1);
        }

        catch
        {
            collectedLightAttackerCount = 0;
        }
        finally
        {
			/*if (collectedLightAttackerCount == 0)
				GameControllerPersistentSingleton.Instance.LostGame();*/
		}
	}
}


#if UNITY_EDITOR

public sealed partial class Player
{ }

#endif