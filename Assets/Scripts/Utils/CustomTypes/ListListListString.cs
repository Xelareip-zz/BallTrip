using UnityEngine;
using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Serializable]
public class ListListListString
{
	[SerializeField]
	private List<ListListString> content = new List<ListListString>(); 
	
	public void Add(ListListString newVal)
	{
		content.Add(newVal);
	}
	
	public ListListString this[int key]
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
}

