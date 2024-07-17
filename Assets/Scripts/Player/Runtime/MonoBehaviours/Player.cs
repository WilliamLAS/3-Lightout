using UnityEngine;

public sealed partial class Player : MonoBehaviour
{
    [SerializeField]
    private uint collectedLightAttackerCount;

    [SerializeField]
    private uint maxCollectableLightAttackerCount;


    // Initialize


    // Update
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


    // Dispose
}


#if UNITY_EDITOR

public sealed partial class Player
{ }

#endif