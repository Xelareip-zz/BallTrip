using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoFirstBuy : TutoBase
{
	public ShopButton selectButton;

	void Update()
	{
		foreach (BUYABLE buyable in Enum.GetValues(typeof(BUYABLE)))
		{
			if (Player.Instance.GetBuyableLevel(buyable) > 0)
			{
				End();
			}
		}
	}

	public override void Begin()
	{
		base.Begin();
		ShopManager.Instance.SelectButton(selectButton);
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
