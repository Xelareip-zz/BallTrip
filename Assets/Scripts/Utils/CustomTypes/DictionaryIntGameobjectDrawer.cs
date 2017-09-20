#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

[CustomPropertyDrawer(typeof(DictionaryIntGameobject))]
public class DictionaryIntGameobjectDrawer : PropertyDrawer
{
	private DictionaryIntGameobject _dict;
	private bool _foldout = true;
	private List<int> _orderedKeys;
	private KeyValuePair<int, int> changedKey;
	private KeyValuePair<int, GameObject> _nextObject;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		CheckInitialize(property, label);
		if (_foldout == false)
			return 17f;
		return _dict.Count * 18f + 94f;
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
			if (GUI.Button(new Rect(new Vector2(position.position.x, position.position.y + yPos), new Vector2(position.size.x, 17f)), "Reorder"))
			{
				_orderedKeys.Sort();
			}
			yPos += 17f;
			EditorGUI.LabelField(new Rect(new Vector2(position.position.x, position.position.y + yPos), new Vector2(position.size.x, 17f)), "Count: " + _dict.Count);
			yPos += 17f;
			EditorGUI.BeginChangeCheck();
			int newIdx = EditorGUI.IntField(new Rect(new Vector2(position.position.x, position.position.y + yPos), new Vector2(position.size.x / 2, 17f)), _nextObject.Key);
			GameObject newObj = EditorGUI.ObjectField(new Rect(new Vector2(position.position.x + position.size.x / 2, position.position.y + yPos), new Vector2(position.size.x / 2, 17f)), _nextObject.Value, typeof(GameObject), false) as GameObject;
			if (EditorGUI.EndChangeCheck())
			{
				_nextObject = new KeyValuePair<int, GameObject>(newIdx, newObj);
				ValueChanged(property);
			}

			yPos += 20f;
			if (GUI.Button(new Rect(new Vector2(position.position.x + position.size.x * 0.1f, position.position.y + yPos), new Vector2(position.size.x * 0.8f, 17f)), "Add"))
			{
				if (_dict.ContainsKey(_nextObject.Key) == false)
				{
					_dict.Add(_nextObject.Key, _nextObject.Value);
					_orderedKeys.Add(_nextObject.Key);
					ValueChanged(property);
				}
				else
				{
					Debug.LogWarning("This dictionary already contains a value for key " + _nextObject.Key);
				}
				return;
			}
			yPos += 6;

			if (Event.current.keyCode == KeyCode.Return && changedKey.Key >= 0)
			{
				if (_orderedKeys.Contains(changedKey.Value))
				{
					Debug.LogWarning("This dictionary already contains a value for key " + changedKey.Value);
				}
				else
				{
                    int oldValue = _orderedKeys[changedKey.Key];
					_orderedKeys[changedKey.Key] = changedKey.Value;
					_dict.Add(changedKey.Value, _dict[oldValue]);
					_dict.Remove(oldValue);
					changedKey = new KeyValuePair<int, int>(-1, -1);
					ValueChanged(property);
				}
				return;
			}

			for (int keyOrder = 0; keyOrder < _orderedKeys.Count; ++keyOrder)
			{
				int key = _orderedKeys[keyOrder];
				GameObject val = _dict[key];
				yPos += 18f;
				if (GUI.Button(new Rect(new Vector2(position.position.x, position.position.y + yPos), new Vector2(17f, 17f)), "-"))
				{
					_dict.Remove(key);
					_orderedKeys.RemoveAt(keyOrder);
					ValueChanged(property);
					return;
				}
				int newKey = EditorGUI.IntField(new Rect(new Vector2(position.position.x + 18f, position.position.y + yPos), new Vector2(position.size.x / 2 - 9f, 17f)), key);

				if (newKey != key)
				{
					changedKey = new KeyValuePair<int, int>(keyOrder, newKey);
				}

				EditorGUI.BeginChangeCheck();
				GameObject newObject = EditorGUI.ObjectField(new Rect(new Vector2(position.position.x + position.size.x / 2 + 9f, position.position.y + yPos), new Vector2(position.size.x / 2 - 9f, 17f)), val, typeof(GameObject), false) as GameObject;
				if (EditorGUI.EndChangeCheck())
				{
					_dict[key] = newObject;
					ValueChanged(property);
					return;
				}
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
			_dict = fieldInfo.GetValue(target) as DictionaryIntGameobject;
			if (_dict == null)
			{
				_dict = new DictionaryIntGameobject();
				fieldInfo.SetValue(target, _dict);
			}
			changedKey = new KeyValuePair<int, int>(-1, -1);
			_orderedKeys = new List<int>();
			foreach (var kvp in _dict)
			{
				_orderedKeys.Add(kvp.Key);
			}
		}
	}
}
#endif