using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRocket : PickupBase
{
	public int value;

	public int endLevel;
	public bool triggered;

	public Vector3 ballVelocity;
	public Collider ballCollider;
	public Rigidbody ballRigidbody;

	void Awake()
	{
		triggered = false;
	}

	void OnTriggerEnter(Collider coll)
	{
		if (Ball.Instance.invincible)
		{
			return;
		}
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			transform.SetParent(null);
			endLevel = InfiniteLevelsManager.Instance.GetFirstLevelNumber() + value;
			Ball.Instance.invincible = true;
			ballRigidbody = Ball.Instance.GetComponent<Rigidbody>();
			ballVelocity = ballRigidbody.velocity;
			ballCollider = Ball.Instance.GetComponent<Collider>();
			ballCollider.isTrigger = true;
			triggered = true;
		}
	}
	
	public override float GetDropWeight()
	{
		return Player.Instance.GetPURocketChances();
	}

	void Update()
	{
		if (triggered == false)
		{
			return;
		}
		if (InfiniteLevelsManager.Instance.GetFirstLevelNumber() >= endLevel)
		{
			Ball.Instance.invincible = false;
			ballCollider.isTrigger = false;
			Destroy(gameObject);
			return;
		}
		Vector3 newVelocity = InfiniteLevelsManager.Instance.GetLevel(InfiniteLevelsManager.Instance.GetFirstLevelNumber() + 1).start.transform.position + Vector3.up - Ball.Instance.transform.position;
		Debug.Log("First level " + InfiniteLevelsManager.Instance.GetFirstLevelNumber());
		Debug.Log(newVelocity.ToString());
		newVelocity = newVelocity.normalized * Ball.Instance.launchSpeed * 1.5f;
		ballRigidbody.velocity = newVelocity;
	}
}
