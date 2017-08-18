using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoFirstLaunch : TutoBase
{
	public float minAngle;
	public float maxAngle;

	void Update()
	{
	}

	public override void Begin()
	{
		InfiniteLevelsManager.Instance.levels[0].ends[0].goalHit += LevelPassed;
		base.Begin();
	}

	public override void End()
	{
		Player.Instance.SetTutoFinished(gameObject.name);
		base.End();
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
