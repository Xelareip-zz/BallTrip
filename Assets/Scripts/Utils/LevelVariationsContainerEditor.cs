#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelVariationsContainer))]
public class LevelVariationsContainerEditor : Editor
{
	private LevelVariationsContainer variations;

	private bool folded;

	private float height;
	
	void OnEnable()
	{
		variations = (LevelVariationsContainer)target;
	}

	public override void OnInspectorGUI()
	{
		EditorGUI.BeginChangeCheck();
		GameObject temp = EditorGUILayout.ObjectField("Level", variations.linkedPrefab, typeof(GameObject), false) as GameObject;
		if (EditorGUI.EndChangeCheck())
		{
			if (temp == null)
			{
				variations.linkedPrefab.GetComponent<InfiniteLevel>().variations = null;
				variations.linkedPrefab = null;
			}
			else
			{
				InfiniteLevel level = temp.GetComponent<InfiniteLevel>();
				if (level != null)
				{
					variations.linkedPrefab = temp;
					level.variations = variations;
				}
			}
        }
	}
}
#endif