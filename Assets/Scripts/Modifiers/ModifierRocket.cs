using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierRocket : IBallModifier
{
	private int endLevel;

	public Vector3 ballVelocity;
	public Collider ballCollider;

	public ModifierRocket(int _endLevel)
	{
		endLevel = _endLevel;
		ballCollider = Ball.Instance.GetComponent<Collider>();
		ballCollider.isTrigger = true;
		Ball.Instance.AddModifier(this);
	}

	public bool OnDamageReceived(ref float value)
	{
		value = 0;
		return true;
	}

	public bool Update()
	{
		if (InfiniteLevelsManager.Instance.GetFirstLevelNumber() >= endLevel)
		{
			ballCollider.isTrigger = false;
			Ball.Instance.RemoveModifier(this);
			return true;
		}
		return true;
	}

	public bool VelocityUpdate(ref Vector3 velocity)
	{
		velocity = InfiniteLevelsManager.Instance.GetLevel(InfiniteLevelsManager.Instance.GetFirstLevelNumber() + 1).start.transform.position + Vector3.up - Ball.Instance.transform.position;
		velocity = velocity.normalized * Ball.Instance.launchSpeed * 1.5f;
		return true;
	}

	public bool OnBallTriggered(Collider coll)
	{
		return false;
	}

	public bool BallTriggerMode()
	{
		return true;
	}
}
