using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
	private static PickupManager instance;
	public static PickupManager Instance
	{
		get
		{
			return instance;
		}
	}

	public List<PickupBase> possiblePickups;

	void Awake ()
	{
		instance = this;
	}

	public GameObject GetRandomPickup()
	{
		float totalWeights = 0;

		for (int pickupIdx = 0; pickupIdx < possiblePickups.Count; ++pickupIdx)
		{
			totalWeights += possiblePickups[pickupIdx].GetDropWeight();
		}

		float randWeight = Random.Range(0, totalWeights);

		for (int pickupIdx = 0; pickupIdx < possiblePickups.Count; ++pickupIdx)
		{
			randWeight -= possiblePickups[pickupIdx].GetDropWeight();
			if (randWeight <= 0)
			{
				if (possiblePickups[pickupIdx] is PickupNone)
				{
					return null;
				}
				return possiblePickups[pickupIdx].gameObject;
            }
		}
		return null;
	}
}
