#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(ShopData))]
public class ShopDataEditor : Editor
{
	private Dictionary<string, Vector2> scrolls = new Dictionary<string, Vector2>();

	private ShopData data;

	private double lastUpdate;

	void OnEnable()
	{
		data = (ShopData)target;
		EditorApplication.update += Update;
	}

	void OnDisable()
	{
		EditorApplication.update -= Update;
	}

	void Update()
	{
		if (EditorApplication.timeSinceStartup - lastUpdate < 0.5f)
		{
			return;
		}
		foreach (BUYABLE buyable in Enum.GetValues(typeof(BUYABLE)))
		{
			if (data.prices.ContainsKey(buyable) == false)
			{
				data.prices.Add(buyable, new List<int>());
			}
		}
	}

	private Vector2 GetScroll(string key)
	{
		if (scrolls.ContainsKey(key) == false)
		{
			scrolls.Add(key, Vector2.zero);
		}
		return scrolls[key];
	}

	private void SetScroll(string key, Vector2 val)
	{
		if (scrolls.ContainsKey(key) == false)
		{
			scrolls.Add(key, val);
		}
		scrolls[key] = val;
	}

	public override void OnInspectorGUI()
	{/*
		EditorGUI.BeginChangeCheck();
		GUILayoutOption[] options = { GUILayout.MaxWidth(50.0f), GUILayout.MinWidth(50.0f) };
		SetScroll("Main", EditorGUILayout.BeginScrollView(GetScroll("Main")));
		foreach (var price in data.prices)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(price.Key.ToString(), GUILayout.MaxWidth(100.0f));
			if (GUILayout.Button("+", options))
			{
				price.Value.Add(0);
				ForceSerialize();
			}
			EditorGUILayout.LabelField("Count " + price.Value.Count, GUILayout.MaxWidth(100.0f));
			EditorGUILayout.EndHorizontal();
			SetScroll(price.Key.ToString(), EditorGUILayout.BeginScrollView(GetScroll(price.Key.ToString()), new GUILayoutOption[]{ GUILayout.MaxHeight(80.0f), GUILayout.MinHeight(80.0f) }));
			EditorGUILayout.BeginHorizontal();
			for (int levelIdx = 0; levelIdx < price.Value.Count; ++levelIdx)
			{
				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField(levelIdx.ToString(), options);
				price.Value[levelIdx] = EditorGUILayout.IntField("", price.Value[levelIdx], options);
				if (GUILayout.Button("-", options))
				{
					price.Value.RemoveAt(levelIdx);
					EditorGUILayout.EndVertical();
					break;
				}
				EditorGUILayout.EndVertical();
			}
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
		EditorGUILayout.EndScrollView();
		if (EditorGUI.EndChangeCheck())
		{
			ForceSerialize();
		}*/
		data = (ShopData)target;
		if (GUILayout.Button("Update data"))
		{
			ManagementWindow.Instance.UpdateShopData();
		}
		if (GUILayout.Button("Update json"))
		{
			ForceSerialize();
		}
		GUILayoutOption[] options = { GUILayout.MaxWidth(50.0f), GUILayout.MinWidth(50.0f) };
		//SetScroll("Main", EditorGUILayout.BeginScrollView(GetScroll("Main")));
		foreach (var price in data.prices)
		{
			EditorGUILayout.BeginVertical();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField(price.Key.ToString(), GUILayout.MaxWidth(100.0f));
			EditorGUILayout.LabelField("Count " + price.Value.Count, GUILayout.MaxWidth(100.0f));
			EditorGUILayout.EndHorizontal();
			//SetScroll(price.Key.ToString(), EditorGUILayout.BeginScrollView(GetScroll(price.Key.ToString()), new GUILayoutOption[] { GUILayout.MaxHeight(80.0f), GUILayout.MinHeight(80.0f) }));
			EditorGUILayout.BeginHorizontal();
			for (int levelIdx = 0; levelIdx < price.Value.Count; ++levelIdx)
			{
				EditorGUILayout.BeginVertical();
				EditorGUILayout.LabelField(levelIdx.ToString(), options);
				EditorGUILayout.LabelField(price.Value[levelIdx].ToString(), options);
				EditorGUILayout.EndVertical();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			//EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();
		}
		//EditorGUILayout.EndScrollView();
	}

	void ForceSerialize()
	{
		FileSystemWatcher watcher = FileWatchersManager.Instance.GetWatcher("ShopData.json");
		watcher.EnableRaisingEvents = false;

		EditorUtility.SetDirty(data);

		string path = "/Data/ShopData.json";
		string finalJson = JsonUtility.ToJson(data);
		StreamWriter writer = new StreamWriter(Application.dataPath + path, false);
		writer.Write(finalJson);
		writer.Close();

		watcher.EnableRaisingEvents = true;
	}
}
#endif