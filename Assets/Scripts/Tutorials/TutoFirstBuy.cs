using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoFirstBuy : TutoBase
{
	void Update()
	{
		if (Player.Instance.GetHearts() > 1)
		{
			End();
		}
	}

	public override void Begin()
	{
		base.Begin();
	}

	public override void End()
	{
		Player.Instance.SetTutoFinished(gameObject.name);
		base.End();
	}

	public override bool GetCanStartGame()
	{
		return false;
	}
}
