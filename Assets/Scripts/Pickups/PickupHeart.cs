using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHeart : PickupBase
{
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.HeartIncrease(1, true);
			Destroy(gameObject);
		}
	}

	public override float GetDropWeight()
	{
		return Player.Instance.GetPUHeartsChances();
	}
}
