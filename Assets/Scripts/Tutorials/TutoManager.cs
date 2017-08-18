using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutoManager : MonoBehaviour
{
	private static TutoManager instance;
	public static TutoManager Instance
	{
		get
		{
			return instance;
		}
	}

	public TutoBase currentTuto;

	public List<TutoBase> tutoList;

	void Awake()
	{
		instance = this;
		SceneManager.sceneLoaded += OnSceneLoaded;
		SceneManager.sceneUnloaded += OnSceneUnloaded;
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		Object[] tutos = Resources.FindObjectsOfTypeAll(typeof(TutoBase));
		for (int tutoIdx = 0; tutoIdx < tutos.Length; ++tutoIdx)
		{
			TutoManager.Instance.tutoList.Add(tutos[tutoIdx] as TutoBase);
		}
	}

	private void OnSceneUnloaded(Scene arg0)
	{
		for (int tutoIdx = tutoList.Count - 1; tutoIdx >= 0; --tutoIdx)
		{
			if (tutoList[tutoIdx] == null)
			{
				tutoList.RemoveAt(tutoIdx);
			}
		}
	}

	public bool StartTuto(string tutoName)
	{
		if (Player.Instance.GetTutoFinished(tutoName))
		{
			return false;
		}

		for (int tutoIdx = 0; tutoIdx < tutoList.Count; ++tutoIdx)
		{
			if (tutoList[tutoIdx].gameObject.name == tutoName)
			{
				tutoList[tutoIdx].Begin();
				return true;
			}
		}
		return false;
	}
	
	public bool GetCanShootAngle(float angle)
	{
		if (angle > 180.0f)
		{
			angle -= 360.0f;
		}
		if (currentTuto != null)
		{
			return currentTuto.GetCanShootAngle(angle);
		}
		return true;
	}
}
