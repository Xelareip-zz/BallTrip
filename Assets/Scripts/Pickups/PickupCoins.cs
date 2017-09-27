using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCoins : PickupBase, ITriggerActivatorReceiver
{
	public float rotationSpeed;

	public override float GetDropWeight()
	{
		return Player.Instance.GetPUCoinsChances();
	}

	void Update()
	{
		transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			PickedUp();
		}
	}

	void PickedUp()
	{
		InfiniteGameManager.Instance.AddCoins(5, true);
		Destroy(gameObject);
	}

	public void Activated()
	{
		PickedUp();
	}
}
