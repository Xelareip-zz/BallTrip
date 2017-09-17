using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRocket : PickupBase
{
	public int value;

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			int endLevel = InfiniteLevelsManager.Instance.GetFirstLevelNumber() + value;
			new ModifierRocket(endLevel);
			Destroy(gameObject);
		}
	}
	
	public override float GetDropWeight()
	{
		// Chances hit 0 at half the best score
		return (1.0f - InfiniteLevelsManager.Instance.currentLevel / Mathf.Max(Player.Instance.GetBestLevel() * 2.0f, 1)) * Player.Instance.GetPURocketChances();
	}
}
