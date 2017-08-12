using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	private static Dictionary<string, LevelManager> levelManagers = new Dictionary<string, LevelManager>();
	public static LevelManager GetLevelManager(string sceneName)
	{
		if (levelManagers.ContainsKey(sceneName))
		{
			return levelManagers[sceneName];
		}
		return null;
	}

	public static LevelManager GetLevelManager(MonoBehaviour caller)
	{
		return GetLevelManager(caller.gameObject.scene.name);
	}

	void Awake()
	{
		levelManagers.Add(gameObject.scene.name, this);
	}

	void OnDestroy()
	{
		levelManagers.Remove(gameObject.scene.name);
	}

	public void GoalReached(Goal goal)
	{
		Debug.Log("Victory");
		SceneManager.LoadScene("MainScene");
	}
}
