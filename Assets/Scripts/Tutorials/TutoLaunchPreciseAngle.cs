using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoLaunchPreciseAngle : TutoBase
{
	public float tolerance;
	public float targetAngle;

	public override void Begin()
	{
		InfiniteLevelsManager.Instance.levels[0].ends[0].goalHit += LevelPassed;
		base.Begin();
	}

	public void LevelPassed(InfiniteLevelGoal goalHit)
	{
		End();
	}

	public override bool GetCanShootAngle(float angle)
	{
		return angle < targetAngle + tolerance && angle > targetAngle - tolerance;
	}

	public override float GetAngleModifier(float angle)
	{
		if (targetAngle > angle - tolerance && targetAngle < angle + tolerance)
		{
			return targetAngle;
		}
		return angle;
	}
}
