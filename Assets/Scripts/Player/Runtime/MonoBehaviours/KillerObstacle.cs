using FMODUnity;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public sealed partial class KillerObstacle : MonoBehaviour, IPooledObject<KillerObstacle>
{
	[Header("KillerObstacle Movement")]
	#region KillerObstacle Movement

	[SerializeField]
	private DirectionType movingDirection;


	#endregion

	[Header("KillerObstacle Enemy")]
	#region KillerObstacle Enemy

	[SerializeField]
	private Enemy enemyController;

	[SerializeField]
	private List<TargetType> lightAcceptedTargetTypeList;

	[SerializeField]
	private List<TargetType> darkAcceptedTargetTypeList;


	#endregion

	[Header("KillerObstacle Visuals")]
	#region KillerObstacle Visuals

	[SerializeField]
	private Transform lightVisual;

	[SerializeField]
	private Transform darkVisual;

	[SerializeField]
	private CinemachineImpulseSource cameraShaker;


	#endregion

	[Header("KillerObstacle Sounds")]
	#region KillerObstacle Sounds

	[SerializeField]
	private StudioEventEmitter idleEmitter;

	[SerializeField]
	private EventReference deathEventReference;


	#endregion

	#region KillerObstacle Other

	public IPool<KillerObstacle> ParentPool
	{ get; set; }


	#endregion


	// Initialize
	private void OnEnable()
	{
		RandomizeLightType();
		idleEmitter.Play();
	}

	public void OnTakenFromPool(IPool<KillerObstacle> pool)
	{ }


	// Dispose
	private void OnDisable()
	{
		idleEmitter.Stop();
	}

	public void ReleaseOrDestroySelf()
	{
		if (ParentPool != null)
			ParentPool.Release(this);
		else
			Destroy(this.gameObject);
	}

	public void OnKilledOtherEnemy(Enemy killed)
	{ }

	public void OnGotKilledByEnemy(Enemy killedBy)
	{
		cameraShaker.GenerateImpulse();
		RuntimeManager.PlayOneShot(deathEventReference, this.transform.position);
		PlayDeathEffect();
		ReleaseOrDestroySelf();
	}

	private void PlayDeathEffect()
	{
		switch (movingDirection)
		{
			case DirectionType.Right:
			KillerObstacleRightDeathEffectSingletonPool.Instance.Get(this.transform.position);
			break;

			case DirectionType.Left:
			KillerObstacleLeftDeathEffectSingletonPool.Instance.Get(this.transform.position);
			break;

			case DirectionType.Up:
			KillerObstacleUpDeathEffectSingletonPool.Instance.Get(this.transform.position);
			break;

			case DirectionType.Down:
			KillerObstacleBottomDeathEffectSingletonPool.Instance.Get(this.transform.position);
			break;

			default:
			Debug.LogError("Please specify moving direction");
			break;
		}
	}

	public void OnReleaseToPool(IPool<KillerObstacle> pool)
	{ }

	public void RandomizeLightType()
	{
		enemyController.acceptedTargetTypeList.Clear();
		lightVisual.gameObject.SetActive(false);
		darkVisual.gameObject.SetActive(false);

		var randomNum = UnityEngine.Random.Range(0, 2);

		if (randomNum == 0)
		{
			enemyController.targetType = TargetType.KillerObstacleDark;
			enemyController.acceptedTargetTypeList.AddRange(darkAcceptedTargetTypeList);
			darkVisual.gameObject.SetActive(true);
		}
		else
		{
			enemyController.targetType = TargetType.KillerObstacleLight;
			enemyController.acceptedTargetTypeList.AddRange(lightAcceptedTargetTypeList);
			lightVisual.gameObject.SetActive(true);
		}
	}
}


#if UNITY_EDITOR

public sealed partial class KillerObstacle
{ }

#endif