using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierShield : IBallModifier
{
	public ModifierShield()
	{
		Ball.Instance.AddModifier(this);
	}

	public bool OnDamageReceived(ref float value)
	{
		value = 0;
		Ball.Instance.RemoveModifier(this);
		return true;
	}

	public bool Update()
	{
		return true;
	}

	public bool VelocityUpdate(ref Vector3 velocity)
	{
		return false;
	}

	public bool OnBallTriggered(Collider coll)
	{
		return false;
	}

	public bool BallTriggerMode()
	{
		return false;
	}
}
