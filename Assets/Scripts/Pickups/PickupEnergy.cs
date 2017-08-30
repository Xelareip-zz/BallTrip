using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupEnergy : PickupBase
{
	public int value;

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.AddHP(value);
			Destroy(gameObject);
		}
	}

	public override float GetDropWeight()
	{
		return Player.Instance.GetPUEnergyChances();
	}
}
