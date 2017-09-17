using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierRocketBoss : IBallModifier
{
	public Transform targetPoint;

	public Vector3 ballVelocity;
	public Collider ballCollider;

	public ModifierRocketBoss(Transform _targetPoint)
	{
		targetPoint = _targetPoint;
		ballCollider = Ball.Instance.GetComponent<Collider>();
		Ball.Instance.AddModifier(this);
	}

	public bool OnDamageReceived(ref float value)
	{
		value = 0;
		return true;
	}

	public bool Update()
	{
		return false;
	}

	public bool VelocityUpdate(ref Vector3 velocity)
	{
		velocity = targetPoint.position - Ball.Instance.transform.position;
		velocity = velocity.normalized * Ball.Instance.launchSpeed * 1.5f;
		return true;
	}

	public bool OnBallTriggered(Collider coll)
	{
		if (coll.gameObject == targetPoint.gameObject)
		{
			targetPoint.gameObject.SetActive(false);
			GameObject.Destroy(targetPoint.gameObject);
            Ball.Instance.RemoveModifier(this);
		}
		return false;
	}

	public bool BallTriggerMode()
	{
		return true;
	}
}
