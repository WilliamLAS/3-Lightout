using UnityEngine;

public sealed partial class LightProtector : MonoBehaviour
{
	[Header("LightProtector Enemy")]
	#region LightProtector Enemy

	[SerializeField]
	private Enemy enemyController;


	#endregion

	[Header("LightProtector Visuals")]
	#region LightProtector Visuals

	[SerializeField]
	private ParticleSystem protectingEffectPS;


	#endregion


	// Initialize


	// Update
	public void StartProtecting()
	{
		enemyController.enabled = true;
		protectingEffectPS.Play();
	}


	public void StopProtecting()
	{
		enemyController.enabled = false;
		protectingEffectPS.Stop();
	}


	// Dispose
}


#if UNITY_EDITOR

public sealed partial class LightProtector
{ }

#endif