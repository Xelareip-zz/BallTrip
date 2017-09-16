using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class FileWatchersManager
{
	private static FileWatchersManager instance;
	public static FileWatchersManager Instance
	{
		get
		{
			return instance;
		}
	}

	static FileWatchersManager()
	{
		if (instance == null)
		{
			new FileWatchersManager();
		}
	}

	Dictionary<string, FileSystemWatcher> watchers;
	

	private FileWatchersManager()
	{
		instance = this;
		watchers = new Dictionary<string, FileSystemWatcher>();
		string path = Application.dataPath + "/Data";
        Debug.Log(path);

		//FileSystemWatcher watcher = new FileSystemWatcher(path);
		//watcher.Filter = "ShopData.asset";
		//watcher.Changed += ShopDataChanged;
		//watcher.EnableRaisingEvents = true;
		//watchers.Add("ShopData.asset", watcher);

		FileSystemWatcher watcher = new FileSystemWatcher(path);
		watcher.Filter = "ShopData.json";
		watcher.Changed += ShopDataChanged;
		watcher.EnableRaisingEvents = true;
		watchers.Add("ShopData.json", watcher);
	}

	public void ShopDataChanged(object sender, FileSystemEventArgs e)
	{
		ManagementWindow.Instance.shouldUpdateShopData = true;
	}

	public FileSystemWatcher GetWatcher(string name)
	{
		if (watchers.ContainsKey(name))
		{
			return watchers[name];
		}
		return null;
	}
}
