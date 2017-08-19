using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoBase : MonoBehaviour
{
	public TutoBase nextTuto;
	public string finishTutorial;
	public bool canDragCamera;
	public bool canEndGame;

	public List<GameObject> linkedUI = new List<GameObject>();

	public virtual void Begin()
	{
		gameObject.SetActive(true);
		TutoManager.Instance.currentTuto = this;

		foreach (GameObject uiObject in linkedUI)
		{
			uiObject.SetActive(true);
		}
	}

	public virtual void End()
	{
		Player.Instance.SetTutoFinished(gameObject.name);
		gameObject.SetActive(false);
		foreach (GameObject uiObject in linkedUI)
		{
			uiObject.SetActive(false);
		}
		if (nextTuto != null)
		{
			nextTuto.Begin();
		}
		else
		{
			TutoManager.Instance.currentTuto = null;
		}
	}

	public virtual float GetAngleModifier(float angle)
	{
		return angle;
	}

	public virtual bool GetCanShootAngle(float angle)
	{
		return true;
	}

	public virtual bool GetCanStartGame()
	{
		return true;
	}

	public virtual bool GetCanDragCamera()
	{
		return canDragCamera;
	}

	public virtual bool GetCanEndGame()
	{
		return canEndGame;
	}
	
}
