using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
	
[Serializable]
public class SpawnRatios : Dictionary<GameObject, DictionaryIntInt>, ISerializationCallbackReceiver
{
	[SerializeField, HideInInspector]
	public List<GameObject> _keys = new List<GameObject>();
	[SerializeField, HideInInspector]
	public List<DictionaryIntInt> _values = new List<DictionaryIntInt>();

	public void OnBeforeSerialize()
	{
		_keys.Clear();
		_values.Clear();


		foreach (var kvp in this)
		{
			_keys.Add(kvp.Key);

			DictionaryIntInt ratiosList = new DictionaryIntInt();
			foreach (var ratio in kvp.Value)
			{
				ratiosList.Add(ratio.Key, ratio.Value);
			}
			_values.Add(ratiosList);
		}
	}

	public void OnAfterDeserialize()
	{
		Clear();
		for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
		{
			DictionaryIntInt ratioDict = new DictionaryIntInt();
			foreach (var kvp in _values[i])
			{
				ratioDict.Add(kvp.Key, kvp.Value);
			}
			Add(_keys[i], ratioDict);
		}
	}
}

