using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShield : PickupBase
{
	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.FreeCollisionsIncrease();
			Destroy(gameObject);
		}
	}

	public override float GetDropWeight()
	{
		if (Player.Instance.GetLevelBeaten(BOSS_TYPE.SHIELD))
		{
			return 30f;
		}
		return 0f;
	}
}
