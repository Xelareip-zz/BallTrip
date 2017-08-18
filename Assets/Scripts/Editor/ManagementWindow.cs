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
	public bool followBall;

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
		followBall = GUILayout.Toggle(followBall, "Follow ball");
		if (GUILayout.Button("Fill levels list"))
		{
			FillLevelsList();
		}
		if (GUILayout.Button("Fill Pickups list"))
		{
			FillPickupsList();
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
		if (InfiniteLevelsDemo.Instance != null)
		{
			if (GUILayout.Button("Fill levels"))
			{
				InfiniteLevelsDemo.Instance.SpawnLevels();
			}
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

		if (followBall)
		{
			if (Ball.Instance != null && SceneView.lastActiveSceneView != null)
			{
				SceneView.lastActiveSceneView.pivot = new Vector3(Ball.Instance.transform.position.x, Ball.Instance.transform.position.y, SceneView.lastActiveSceneView.pivot.z);
			}
		}
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

	private void FillLevelsList()
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
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}

	private void FillPickupsList()
	{
		PickupManager manager = GameObject.FindObjectOfType<PickupManager>();
		if (manager == null)
		{
			Debug.LogWarning("No PickupManager found!");
			return;
		}
		manager.possiblePickups.Clear();
		foreach (string assetGUID in AssetDatabase.FindAssets("t:GameObject"))
		{
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetGUID));
			PickupBase pickup = go.GetComponent<PickupBase>();
			if (pickup && pickup.active)
			{
				manager.possiblePickups.Add(pickup);
			}
			continue;
		}
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}
}