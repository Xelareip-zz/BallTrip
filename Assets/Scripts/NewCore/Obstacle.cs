using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IObstacle
{
	public int hpCost;

	void Awake()
	{
		foreach (CollisionSignal signal in GetComponentsInChildren<CollisionSignal>())
		{
			signal.collisionEnter += Signal_collisionEnter;
		}
		gameObject.SetActive(false);
	}

	private void Signal_collisionEnter(CollisionSignal signal, Collision coll)
	{
		OnCollisionEnter(coll);
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.Hit(this);
		}
	}
	
	public float HpLossOnTick()
	{
		return hpCost;
	}
}
