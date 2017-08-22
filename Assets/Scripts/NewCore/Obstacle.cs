using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	public int heartCost;

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.Hit(this, coll);
		}
	}
}
