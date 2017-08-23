using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclePoison : MonoBehaviour, IObstacle
{
	public float hpPerSecond;

	void Awake()
	{
		foreach (CollisionTrigger trigger in GetComponentsInChildren<CollisionTrigger>())
		{
			trigger.collisionStay += Signal_collisionStay;
		}
		gameObject.SetActive(false);
	}

	private void Signal_collisionStay(CollisionTrigger trigger, Collider coll)
	{
		OnTriggerStay(coll);
	}

	void OnTriggerStay(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.Hit(this);
		}
	}

	public float HpLossOnTick()
	{
		return hpPerSecond * Time.fixedDeltaTime;
	}
}
