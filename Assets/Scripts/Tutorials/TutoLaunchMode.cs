using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoLaunchMode : TutoBase
{
	public LAUNCH_MODE launchMode;

	void Update()
	{
		if (launchMode != InfiniteGameManager.Instance.GetMode())
		{
			End();
		}
	}

	public override void Begin()
	{
		base.Begin();
		Time.timeScale = 0.0f;
	}

	public override void End()
	{
		Time.timeScale = 1.0f;
		Player.Instance.SetTutoFinished(gameObject.name);
		base.End();
	}
}
