#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEditor;

[CustomPropertyDrawer(typeof(DictionaryIntInt))]
public class DictionaryIntIntDrawer : PropertyDrawer
{
	public DictionaryIntInt _dict;
	private bool _foldout = false;
	private List<int> _orderedKeys = new List<int>();
	private KeyValuePair<int, int> changedKey;
	private KeyValuePair<int, int> _nextObject;

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		CheckInitialize(property, label);
		if (_foldout == false)
			return 17f;
		return _dict.Count * 18f + 77f;
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
			int newIdx = EditorGUI.IntField(new Rect(new Vector2(position.position.x, position.position.y + yPos), new Vector2(position.size.x / 2, 17f)), _nextObject.Key);
			int newObj = EditorGUI.IntField(new Rect(new Vector2(position.position.x + position.size.x / 2, position.position.y + yPos), new Vector2(position.size.x / 2, 17f)), _nextObject.Value);
			if (EditorGUI.EndChangeCheck())
			{
				_nextObject = new KeyValuePair<int, int>(newIdx, newObj);
				ValueChanged();
			}

			yPos += 20f;
			if (GUI.Button(new Rect(new Vector2(position.position.x + position.size.x * 0.1f, position.position.y + yPos), new Vector2(position.size.x * 0.8f, 17f)), "Add"))
			{
				if (_dict.ContainsKey(_nextObject.Key) == false)
				{
					_dict.Add(_nextObject.Key, _nextObject.Value);
					_orderedKeys.Add(_nextObject.Key);
					ValueChanged();
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
					ValueChanged();
				}
				return;
			}

			for (int keyOrder = 0; keyOrder < _orderedKeys.Count; ++keyOrder)
			{
				int key = _orderedKeys[keyOrder];
				int val = _dict[key];
				yPos += 18f;
				if (GUI.Button(new Rect(new Vector2(position.position.x, position.position.y + yPos), new Vector2(17f, 17f)), "-"))
				{
					_dict.Remove(key);
					_orderedKeys.RemoveAt(keyOrder);
					ValueChanged();
					return;
				}
				int newKey = EditorGUI.IntField(new Rect(new Vector2(position.position.x + 18f, position.position.y + yPos), new Vector2(position.size.x / 2 - 9f, 17f)), key);

				if (newKey != key)
				{
					changedKey = new KeyValuePair<int, int>(keyOrder, newKey);
				}

				EditorGUI.BeginChangeCheck();
				int newObject = EditorGUI.IntField(new Rect(new Vector2(position.position.x + position.size.x / 2 + 9f, position.position.y + yPos), new Vector2(position.size.x / 2 - 9f, 17f)), val);
				if (EditorGUI.EndChangeCheck())
				{
					_dict[key] = newObject;
					ValueChanged();
					return;
				}
			}
		}
	}

	private void ValueChanged()
	{
		EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
	}

	private void CheckInitialize(SerializedProperty property, GUIContent label)
	{
		if (_dict == null)
		{
			var target = property.serializedObject.targetObject;
			_dict = fieldInfo.GetValue(target) as DictionaryIntInt;
			if (_dict == null)
			{
				_dict = new DictionaryIntInt();
				fieldInfo.SetValue(target, _dict);
			}
			changedKey = new KeyValuePair<int, int>(-1, -1);
		}
		if (_orderedKeys.Count != _dict.Count)
		{
			_orderedKeys = new List<int>();
			foreach (var kvp in _dict)
			{
				_orderedKeys.Add(kvp.Key);
			}
		}
	}
}
#endif