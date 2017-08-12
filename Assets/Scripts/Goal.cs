using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
	void Awake()
	{
		foreach (CollisionTrigger trigger in GetComponentsInChildren<CollisionTrigger>())
		{
			trigger.collisionEnter += OnTriggerEnter;
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		Ball ball = coll.GetComponent<Ball>();
		if (ball != null)
		{
			LevelManager.GetLevelManager(this).GoalReached(this);
		}
	}
}
