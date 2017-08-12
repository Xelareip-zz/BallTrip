using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
	
[Serializable]
public class DictionaryIntInt : Dictionary<int, int>, ISerializationCallbackReceiver
{
	[SerializeField, HideInInspector]
	public List<int> _keys = new List<int>();
	[SerializeField, HideInInspector]
	public List<int> _values = new List<int>();

	public void OnBeforeSerialize()
	{
		_keys.Clear();
		_values.Clear();


		foreach (KeyValuePair<int, int> kvp in this)
		{
			_keys.Add(kvp.Key);
			_values.Add(kvp.Value);
		}
	}

	public void OnAfterDeserialize()
	{
		Clear();
		for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
		{
			Add(_keys[i], _values[i]);
		}
	}
}

