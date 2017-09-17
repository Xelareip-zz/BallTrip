using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShopData : ScriptableObject, ISerializationCallbackReceiver
{
	public Dictionary<BUYABLE, List<int>> prices = new Dictionary<BUYABLE, List<int>>();

	[SerializeField, HideInInspector]
	public List<string> _keys;
	[SerializeField, HideInInspector]
	public List<string> _values;


	public void OnBeforeSerialize()
	{
		_keys.Clear();
		_values.Clear();

		foreach (KeyValuePair<BUYABLE, List<int>> kvp in prices)
		{
			string endString = "";

			for (int priceIdx = 0; priceIdx < kvp.Value.Count; ++priceIdx)
			{
				endString += ":" + kvp.Value[priceIdx];
			}
			if (string.IsNullOrEmpty(endString) == false)
			{
				endString = endString.Remove(0, 1);
			}

			_keys.Add(kvp.Key.ToString());
			_values.Add(endString);
		}
	}

	public void OnAfterDeserialize()
	{
		prices.Clear(); 
		for (int i = 0; i != Mathf.Min(_keys.Count, _values.Count); i++)
		{
			string[] stringArray = _values[i].Split(':');
            List<int> pricesList = new List<int>();

			if (stringArray.Length > 0)
			{
				foreach (string str in stringArray)
				{
					if (string.IsNullOrEmpty(str) == false)
					{
						pricesList.Add(int.Parse(str));
					}
				}
			}

			prices.Add((BUYABLE)Enum.Parse(typeof(BUYABLE), _keys[i]), pricesList);
		}
	}

}
