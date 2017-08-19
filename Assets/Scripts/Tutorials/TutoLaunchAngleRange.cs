using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoLaunchAngleRange : TutoBase
{
	public float minAngle;
	public float maxAngle;

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
		return angle < maxAngle && angle > minAngle;
	}
}
