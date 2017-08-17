#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[ExecuteInEditMode]
public class InfiniteLevelsDemo : MonoBehaviour
{
	private static InfiniteLevelsDemo instance;
	public static InfiniteLevelsDemo Instance
	{
		get
		{
			return instance;
		}
	}

	public GameObject root;

	void Awake()
	{
		instance = this;
	}

	public void SpawnLevels()
	{
		while (root.transform.childCount != 0)
		{
			GameObject toDestroy = root.transform.GetChild(0).gameObject;
            toDestroy.transform.SetParent(null);
			DestroyImmediate(toDestroy);
		}

		int x = 0;
		int y = 0;
		foreach (string assetGUID in AssetDatabase.FindAssets("t:GameObject"))
		{
			x += 35;
			if (x >= 100)
			{
				x = 0;
				y += 50;
			}
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetGUID));
			InfiniteLevel level = go.GetComponent<InfiniteLevel>();
			if (level != null)
			{
				GameObject newLevel = PrefabUtility.InstantiatePrefab(go) as GameObject;
				newLevel.transform.SetParent(root.transform);
				newLevel.transform.position = new Vector3(x, y, 0);
			}
			continue;
		}
		EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
	}
}
#endif