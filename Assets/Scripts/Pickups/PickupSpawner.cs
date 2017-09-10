using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
	public List<GameObject> pickups;

	public List<Vector3> pickupSpots;

	void Awake()
	{
		for (int idx = 0; idx < pickups.Count; ++idx)
		{
			PickupBase pu = pickups[idx].GetComponentInChildren<PickupBase>();

			if (Random.Range(0, 100) < pu.GetDropWeight())
			{
				Transform parent = transform.GetChild(Random.Range(0, transform.childCount));
                Instantiate(pickups[idx], parent, false);
				break;
			}
		}
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		for (int childIdx = 0; childIdx < transform.childCount; ++childIdx)
		{
			Gizmos.DrawSphere(transform.GetChild(childIdx).position, 0.3f);
		}
	}
#endif
}
