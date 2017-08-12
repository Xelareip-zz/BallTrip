#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


public class ManagementWindow : EditorWindow
{
	[SerializeField]
	public string lastScene;
	[SerializeField]
	public bool wasPlaying;
	public string screenshotName = "";

	[MenuItem("BallTrip/Management")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		ManagementWindow window = (ManagementWindow)GetWindow(typeof(ManagementWindow));
		window.Show();
	}

	void OnGUI()
	{
		GUILayout.BeginVertical();
		if (GUILayout.Button("Fill levels list"))
		{
			InfiniteLevelsManager manager = GameObject.FindObjectOfType<InfiniteLevelsManager>();
			if (manager == null)
			{
				Debug.LogWarning("No InfiniteLevelsManager found!");
				return;
			}
			manager.possibleLevels.Clear();
			foreach (string assetGUID in AssetDatabase.FindAssets("t:GameObject"))
			{
				GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetGUID));
				InfiniteLevel level = go.GetComponent<InfiniteLevel>();
				if (level && level.baseLevel)
				{
					manager.possibleLevels.Add(go);
				}
				continue;
			}
		}
		if (LevelPieceFactory.Instance != null)
		{
			if (GUILayout.Button("Reload level pieces"))
			{
				LevelPieceFactory.Instance.ReloadLevelPiecesList();
			}
        }
		else
		{
			GUILayout.Label("No level piece factory in the scene!");
		}
		if (GUILayout.Button("Update all bounds"))
		{
			UpdateAllBounds();
		}
		screenshotName = GUILayout.TextField(screenshotName);
		if (GUILayout.Button("Screenshot"))
		{
			ScreenCapture.CaptureScreenshot(screenshotName);
		}
		if (GUILayout.Button("Start game"))
		{
			StartGame();
		}
		GUILayout.EndVertical();
	}

	void Update()
	{
		if (wasPlaying && EditorApplication.isPlaying == false)
		{
			EditorSceneManager.OpenScene(lastScene);
		}
		wasPlaying = EditorApplication.isPlaying;
	}

	private void UpdateAllBounds()
	{
		foreach (string assetGUID in AssetDatabase.FindAssets("t:GameObject"))
		{
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetGUID));
			InfiniteLevel level = go.GetComponent<InfiniteLevel>();
			if (level != null)
			{
				level.CalculateBounds();
			}
            continue;
		}
	}

	private void StartGame()
	{
		lastScene = EditorSceneManager.GetActiveScene().path;
		EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		EditorSceneManager.OpenScene("Assets/Scenes/LaunchScene.unity");
		EditorApplication.isPlaying = true;
	}
}
#endif