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
	public GUISkin skin;

	[MenuItem("BallTrip/Management")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		ManagementWindow window = (ManagementWindow)GetWindow(typeof(ManagementWindow));
		window.Show();
	}

	private void InitSkin()
	{
		if (skin == null)
		{
			skin = AssetDatabase.LoadAssetAtPath<GUISkin>("Assets/EditorSkin/XelareipSkin.guiskin");
		}
		GUI.skin = skin;
	}

	void OnGUI()
	{
		InitSkin();
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
		if (GUILayout.Button("Set all GUIDs"))
		{
			SetAllGUIDs();
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
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Start game"))
		{
			StartGame();
		}
		if (GUILayout.Button("Start new game"))
		{
			DeleteSave();
			StartGame();
		}
		GUILayout.EndHorizontal();
		if (InfiniteLevelsDemo.Instance != null)
		{
			if (GUILayout.Button("Fill levels"))
			{
				InfiniteLevelsDemo.Instance.SpawnLevels();
			}
		}
		if (GUILayout.Button("Save selected prefabs"))
		{
			SaveSelectedPrefabs();
		}
		GUILayout.EndVertical();
	}

	void Update()
	{
		if (wasPlaying && EditorApplication.isPlaying == false && string.IsNullOrEmpty(lastScene) == false)
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
		foreach (InfiniteLevel level in FindObjectsOfType<InfiniteLevel>())
		{
			level.CalculateBounds();
		}
	}

	private void StartGame()
	{

		VariationsWindow window = (VariationsWindow)GetWindow(typeof(VariationsWindow));
		if (window != null)
		{
			window.SetActive(false);
		}
		else
		{
			Debug.Log("No variations window");
		}
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

	private void SetAllGUIDs()
	{
		foreach (string assetGUID in AssetDatabase.FindAssets("t:GameObject"))
		{
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetGUID));
			IGUIDIdentified[] guidHolders = go.GetComponentsInChildren<IGUIDIdentified>();
			foreach (var guidHolder in guidHolders)
			{
				guidHolder.SetGUID();
			}
		}
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

	private void SaveSelectedPrefabs()
	{
		foreach (var obj in Selection.gameObjects)
		{
			PrefabUtility.ReplacePrefab(obj, PrefabUtility.GetPrefabParent(obj), ReplacePrefabOptions.Default);
			(PrefabUtility.GetPrefabParent(obj) as GameObject).transform.position = Vector3.zero;
        }
	}

	private void DeleteSave()
	{
		FileUtil.DeleteFileOrDirectory(Player.GetPath());
	}
}