#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorRenderers
{
	public static void RenderDictionary(Dictionary<string, GameObject> dict)
	{
		foreach (var kvp in dict)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.ObjectField(kvp.Key, kvp.Value, typeof(GameObject), false);
			EditorGUILayout.EndHorizontal();
		}
	}

	public static void RenderDictionary(Dictionary<int, GameObject> dict)
	{
		foreach (var kvp in dict)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.IntField(kvp.Key);
			EditorGUILayout.ObjectField("", kvp.Value, typeof(GameObject), false);
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif