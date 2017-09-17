using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupRocketBoss : MonoBehaviour
{
	public Collider target;

	public TriggerSignal triggerSignal;

	void Awake()
	{
		triggerSignal.collisionEnter += OnTriggerEnter;
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			target.gameObject.layer = LayerMask.NameToLayer("Default");
			new ModifierRocketBoss(target.transform);
			Destroy(gameObject);
		}
	}

#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (target != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(transform.position, target.transform.position);
		}
	}
#endif
}
