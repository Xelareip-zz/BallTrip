using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MetaManager : MonoBehaviour
{
	private static MetaManager instance;
	public static MetaManager Instance
	{
		get
		{
			return instance;
		}
	}

	public LevelDisplayer levelDisplayer;

	public string levelsLocation;
	public List<string> levels;

	void Awake()
	{
		instance = this;
	}

	void Start()
	{
		levels = new List<string>();
		for (int sceneId = 0; sceneId < SceneManager.sceneCountInBuildSettings; ++sceneId)
		{
			string sceneName = SceneUtility.GetScenePathByBuildIndex(sceneId);
			if (sceneName.StartsWith(levelsLocation))
			{
				levels.Add(sceneName);
			}
		}

		levelDisplayer.SetUIForLevels(levels);
	}
}
