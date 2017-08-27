#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelVariationsContainer))]
public class LevelVariationsContainerEditor : Editor
{
	private List<GameObject> possibleLevels = new List<GameObject>();
	private LevelVariationsContainer variationsData;

	private double lastUpdate;
	
	void OnEnable()
	{
		variationsData = (LevelVariationsContainer)target;
		EditorApplication.update += Update;
	}

	void Update()
	{
		if (EditorApplication.timeSinceStartup - lastUpdate < 0.5f)
		{
			return;
		}
		lastUpdate = EditorApplication.timeSinceStartup;
		FindLevels();
		Repaint();
	}

	private void FindLevels()
	{
		possibleLevels.Clear();
		string[] assetsPaths = AssetDatabase.FindAssets("t:GameObject");

		for (int assetIdx = 0; assetIdx < assetsPaths.Length; ++assetIdx)
		{
			GameObject newObject = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(assetsPaths[assetIdx]));
			InfiniteLevel level = newObject.GetComponentInChildren<InfiniteLevel>();
			if (level != null)
			{
				possibleLevels.Add(newObject);
			}
		}
	}

	public override void OnInspectorGUI()
	{
		int idx = possibleLevels.IndexOf(variationsData.linkedPrefab);
		List<string> names = new List<string>();
		names.Add("None");
		foreach (var level in possibleLevels)
		{
			names.Add(level.name);
		}
		EditorGUI.BeginChangeCheck();
		int newIndex = EditorGUILayout.Popup("Current level", idx + 1, names.ToArray());

		if (EditorGUI.EndChangeCheck())
		{
			if (newIndex > 0)
			{
				variationsData.linkedPrefab = possibleLevels[newIndex - 1];
				variationsData.linkedPrefab.GetComponent<InfiniteLevel>().variationsData = variationsData;
			}
			else
			{
				variationsData.linkedPrefab.GetComponent<InfiniteLevel>().variationsData = null;
				variationsData.linkedPrefab = null;
			}
		}

		EditorGUI.BeginChangeCheck();
		GameObject temp = EditorGUILayout.ObjectField("Level", variationsData.linkedPrefab, typeof(GameObject), false) as GameObject;
		if (EditorGUI.EndChangeCheck())
		{
			if (temp == null)
			{
				variationsData.linkedPrefab.GetComponent<InfiniteLevel>().variationsData = null;
				variationsData.linkedPrefab = null;
			}
			else
			{
				InfiniteLevel level = temp.GetComponent<InfiniteLevel>();
				if (level != null)
				{
					variationsData.linkedPrefab = temp;
					level.variationsData = variationsData;
				}
			}
		}

		//variationsData.baseObstaclesCountLevel = EditorGUILayout.IntField("Start spawn level", variationsData.baseObstaclesCountLevel);
		//variationsData.levelsToAllObstacles = EditorGUILayout.IntField("Levels to max difficulty", variationsData.levelsToAllObstacles);

		for (int layerIdx = 0; layerIdx < variationsData.layerLevels.Count; ++layerIdx)
		{
			EditorGUILayout.LabelField("Layer " + layerIdx + "  " + variationsData.GetLevelForLayer(layerIdx));
		}
	}
}
#endif