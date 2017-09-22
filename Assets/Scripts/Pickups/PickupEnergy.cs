using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEnergy : PickupBase
{
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.AddHP(InfiniteGameManager.Instance.GetObstacleDamage());
			Destroy(gameObject);
		}
	}

	public override float GetDropWeight()
	{
		return Player.Instance.GetPUEnergyChances();
	}
}
