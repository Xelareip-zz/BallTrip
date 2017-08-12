#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

[CustomPropertyDrawer(typeof(SpawnRatios))]
public class SpawnRatiosDrawer : PropertyDrawer
{
	private SpawnRatios _dict;
	private Dictionary<GameObject, DictionaryIntIntDrawer> _drawers = new Dictionary<GameObject, DictionaryIntIntDrawer>();
	private bool _foldout = true;
	private List<GameObject> _orderedKeys;
	private GameObject _nextObject;


	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		CheckInitialize(property, label);
		if (_foldout == false)
			return 17f;
		float finalHeight = 0f;
		foreach (var kvp in _drawers)
		{
			finalHeight += kvp.Value.GetPropertyHeight(null, label) + 25f;
		}
		return finalHeight + 71f;
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		CheckInitialize(property, label);


		var foldoutRect = position;
		foldoutRect.width -= 2 * 50;
		foldoutRect.height = 17f;
		EditorGUI.BeginChangeCheck();
		_foldout = EditorGUI.Foldout(foldoutRect, _foldout, label, true);
		if (EditorGUI.EndChangeCheck())
			EditorPrefs.SetBool(label.text, _foldout);

		float yPos = 17f;

		if (_foldout)
		{
			EditorGUI.LabelField(new Rect(new Vector2(position.position.x, position.position.y + yPos), new Vector2(position.size.x, 17f)), "Count: " + _dict.Count);
			yPos += 17f;
			EditorGUI.BeginChangeCheck();
			GameObject newObj = EditorGUI.ObjectField(new Rect(new Vector2(position.position.x + position.size.x / 2, position.position.y + yPos), new Vector2(position.size.x / 2, 17f)), _nextObject, typeof(GameObject), false) as GameObject;
			if (EditorGUI.EndChangeCheck())
			{
				_nextObject = newObj;
				ValueChanged(property);
			}

			yPos += 20f;
			if (GUI.Button(new Rect(new Vector2(position.position.x + position.size.x * 0.1f, position.position.y + yPos), new Vector2(position.size.x * 0.8f, 17f)), "Add"))
			{
				if (_nextObject == null)
				{
					Debug.LogWarning("Cannot add empty gameobject here");
					return;
				}
				else if (_dict.ContainsKey(_nextObject) == false)
				{
					_dict.Add(_nextObject, new DictionaryIntInt());
					DictionaryIntIntDrawer newDrawer = new DictionaryIntIntDrawer();
					newDrawer._dict = _dict[_nextObject];
                    _drawers.Add(_nextObject, newDrawer);
					_orderedKeys.Add(_nextObject);
					ValueChanged(property);
				}
				else
				{
					Debug.LogWarning("This dictionary already contains a value for key " + _nextObject.name);
				}
				return;
			}
			yPos += 17f;

			for (int keyOrder = 0; keyOrder < _orderedKeys.Count; ++keyOrder)
			{
				GameObject key = _orderedKeys[keyOrder];
				DictionaryIntInt val = _dict[key];
				yPos += 8f;
				if (GUI.Button(new Rect(new Vector2(position.position.x, position.position.y + yPos), new Vector2(17f, 17f)), "-"))
				{
					_dict.Remove(key);
                    _drawers.Remove(key);
					_orderedKeys.RemoveAt(keyOrder);
					ValueChanged(property);
					return;
				}
				GameObject newObject = EditorGUI.ObjectField(new Rect(new Vector2(position.position.x + 18f, position.position.y + yPos), new Vector2(position.size.x / 2 - 9f, 17f)), key, typeof(GameObject), false) as GameObject;
				if (newObject == null)
				{
					return;
				}
				if (newObject != key)
				{
					int idx = _orderedKeys.FindIndex((o) => o == key);
					GameObject oldValue = _orderedKeys[idx];
					_orderedKeys[idx] = newObject;
					_dict.Add(newObject, _dict[oldValue]);
					_dict.Remove(oldValue);
					_drawers.Add(newObject, _drawers[oldValue]);
					_drawers.Remove(oldValue);
					ValueChanged(property);
					return;
				}
				GUIContent guiContent = new GUIContent("Values");
				yPos += 17f;
				_drawers[key].OnGUI(new Rect(new Vector2(position.position.x + 17f, position.position.y + yPos), new Vector2(position.size.x - 17f, _drawers[key].GetPropertyHeight(null, guiContent))), null, guiContent);
				yPos += _drawers[key].GetPropertyHeight(null, guiContent);
            }
		}
	}

	private void ValueChanged(SerializedProperty property)
	{
		//var target = property.serializedObject.targetObject;
		//fieldInfo.SetValue(target, _dict);
		EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
	}

	private void CheckInitialize(SerializedProperty property, GUIContent label)
	{
		if (_dict == null)
		{
			var target = property.serializedObject.targetObject;
			_dict = fieldInfo.GetValue(target) as SpawnRatios;
			if (_dict == null)
			{
				_dict = new SpawnRatios();
				fieldInfo.SetValue(target, _dict);
			}
			_orderedKeys = new List<GameObject>();
			foreach (var kvp in _dict)
			{
				_orderedKeys.Add(kvp.Key);
				DictionaryIntIntDrawer newDrawer = new DictionaryIntIntDrawer();
				newDrawer._dict = kvp.Value;
				_drawers.Add(kvp.Key, newDrawer);
			}
		}
	}
}
#endif