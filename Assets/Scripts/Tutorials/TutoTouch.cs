using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoTouch : TutoBase
{
	void Update()
	{
		//Time.timeScale = Mathf.Min(Time.timeScale + Time.deltaTime / Time.timeScale, 1.0f);

		foreach (Touch touch in Input.touches)
		{
			if (touch.phase == TouchPhase.Began)
			{
				End();
			}
		}

		if (Input.GetMouseButtonDown(0))
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
