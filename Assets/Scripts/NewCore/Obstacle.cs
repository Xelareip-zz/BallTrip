using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IObstacle
{
	#region GUID_IDENTIFIED
	[SerializeField]
	private string GUID = null;

	public void SetGUID()
	{
		if (GetGUID() != null)
		{
			GUID = Guid.NewGuid().ToString();
		}
	}

	public string GetGUID()
	{
		return GUID;
	}
	#endregion

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

	void OnTriggerEnter(Collider coll)
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
