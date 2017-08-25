#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

[CustomPropertyDrawer(typeof(LevelVariationsContainer))]
public class LevelVariationsContainerDrawer : PropertyDrawer
{
	private float height;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		return 17f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		LevelVariationsContainer variations = fieldInfo.GetValue(property.serializedObject.targetObject) as LevelVariationsContainer;
		
		height = 0;
		EditorGUI.BeginChangeCheck();
		string name = property.name.Substring(0, 1).ToUpper() + property.name.Substring(1);
        LevelVariationsContainer temp = EditorGUI.ObjectField(new Rect(new Vector2(position.position.x, position.position.y + height), new Vector2(position.size.x, 17f)), name, (property.serializedObject.targetObject as InfiniteLevel).variations, typeof(LevelVariationsContainer), false) as LevelVariationsContainer;
		if (EditorGUI.EndChangeCheck())
		{
			if (temp == null)
			{
				if (variations)
				{
					variations.linkedPrefab = null;
					EditorUtility.SetDirty(variations);
				}
				(property.serializedObject.targetObject as InfiniteLevel).variations = null;

				EditorUtility.SetDirty((property.serializedObject.targetObject as InfiniteLevel).gameObject);
			}
			else
			{
				(property.serializedObject.targetObject as InfiniteLevel).variations = temp;
				temp.linkedPrefab = (property.serializedObject.targetObject as InfiniteLevel).gameObject;
				EditorUtility.SetDirty((property.serializedObject.targetObject as InfiniteLevel).variations);
				EditorUtility.SetDirty(temp.linkedPrefab);
			}
		}

	}
}
#endif