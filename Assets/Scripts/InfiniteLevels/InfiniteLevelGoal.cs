using System.Collections.Generic;
using UnityEngine;
using System;

public class InfiniteLevelGoal : MonoBehaviour
{
	public Action<InfiniteLevelGoal> goalHit;

	public InfiniteLevel level;
	public InfiniteLevelStart boundStart;

	void OnTriggerExit(Collider coll)
	{
		GoalExited(coll);
	}

	public void GoalExited(Collider collider)
	{
		if (collider.gameObject == Ball.Instance.gameObject && collider.transform.position.y > transform.position.y)
		{
			if (goalHit != null)
			{
				goalHit(this);
			}
		}
	}
}
