using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ListString
{
	[SerializeField]
	private List<string> content = new List<string>(); 
	
	public void Add(string newVal)
	{
		content.Add(newVal);
	}
	
	public string this[int key]
	{
		get
		{
			return content[key];
		}
		set
		{
			content[key] = value;
		}
	}

	public void RemoveAt(int index)
	{
		content.RemoveAt(index);
	}

	public int Count
	{
		get
		{
			return content.Count;
		}
	}

	public bool Contains(string val)
	{
		return content.Contains(val);
	}

	public bool Remove(string val)
	{
		return content.Remove(val);
	}
}

