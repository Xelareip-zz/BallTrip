using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ListListString
{
	[SerializeField]
	private List<ListString> content = new List<ListString>(); 
	
	public void Add(ListString newVal)
	{
		content.Add(newVal);
	}
	
	public ListString this[int key]
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

	public bool Contains(ListString val)
	{
		return content.Contains(val);
	}
}

