using UnityEngine;
using System;
using System.Collections.Generic;

public class TutoBase : MonoBehaviour
{
	public TutoBase nextTuto;
	public string finishTutorial;

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

	public virtual bool GetCanShootAngle(float angle)
	{
		return true;
	}
}
