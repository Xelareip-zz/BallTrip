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

	public List<GameObject> possiblePickups;

	void Awake ()
	{
		instance = this;
	}

	public GameObject GetRandomPickup()
	{
		if (Random.Range(0, 100) > 70)
		{
			return possiblePickups[Random.Range(0, possiblePickups.Count)];
		}
		return null;
	}
}
