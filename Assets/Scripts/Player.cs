using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
	private static Player instance;
	public static Player Instance
	{
		get
		{
			return instance;
		}
	}

	public int _bestScore = 0;
	public int _gamesPlayed = 0;
	public int _coins = 0;
	public int _launchesAllowed = 10;
	public int _viewRange = 2;
	public int _bounces = 1;

	public List<string> _bossLevelsBeaten = new List<string>();
	public List<string> _tutoFinished = new List<string>();

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		Load();
	}

	public int GetBestScore()
	{
		return _bestScore;
	}

	public void UpdateBestScore(int score)
	{
		if (score > _bestScore)
		{
			_bestScore = score;
			Save();
		}
	}

	public void NewGameStarted()
	{
		++_gamesPlayed;
	}

	public int GetGamesStarted()
	{
		return _gamesPlayed;
	}

	public void AddCoins(int val)
	{
		_coins += val;
		Save();
	}

	public void SetLaunchesAllowed(int val)
	{
		_launchesAllowed = val;
		Save();
	}

	public void SetViewRange(int val)
	{
		_viewRange = val;
		Save();
	}

	public void SetBounces(int val)
	{
		_bounces = val;
		Save();
	}

	public void SetLevelBeaten(int level)
	{
		SetLevelBeaten(level.ToString());
	}

	public void SetLevelBeaten(BOSS_TYPE boss)
	{
		_bossLevelsBeaten.Add(boss.ToString());
		Save();
	}

	public void SetLevelBeaten(string level)
	{
		_bossLevelsBeaten.Add(level);
		Save();
	}

	public void SetTutoFinished(string tuto)
	{
		_tutoFinished.Add(tuto);
		Save();
	}

	public int GetCoins()
	{
		return _coins;
	}

	public int GetLaunches()
	{
		return _launchesAllowed;
	}

	public int GetViewRange()
	{
		return _viewRange;
	}

	public int GetBounces()
	{
		return _bounces;
	}

	public bool GetLevelBeaten(int level)
	{
		return _bossLevelsBeaten.Contains(level.ToString());
	}

	public bool GetLevelBeaten(BOSS_TYPE boss)
	{
		return _bossLevelsBeaten.Contains(boss.ToString());
	}

	public bool GetLevelBeaten(string level)
	{
		return _bossLevelsBeaten.Contains(level);
	}

	public bool GetTutoFinished(string tuto)
	{
		return _tutoFinished.Contains(tuto);
	}

	public void ResetLevels()
	{
		_bossLevelsBeaten.Clear();
		Save();
	}

	private void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/Save.dat") == false)
		{
			return;
		}
		StreamReader reader = new StreamReader(Application.persistentDataPath + "/Save.dat");
		string saveString = reader.ReadToEnd();
		reader.Close();
		saveString = saveString.Replace("\r", "");
		string[] saveLines = saveString.Split('\n');

		foreach (string line in saveLines)
		{
			string[] lineSplit = line.Split(':');
			switch (lineSplit[0])
			{
				case "bestScore":
					_bestScore = int.Parse(lineSplit[1]);
					break;
				case "gamesPlayed":
					_gamesPlayed = int.Parse(lineSplit[1]);
					break;
				case "coins":
					_coins = int.Parse(lineSplit[1]);
					break;
				case "launchesAllowed":
					_launchesAllowed = int.Parse(lineSplit[1]);
					break;
				case "viewRange":
					_viewRange = int.Parse(lineSplit[1]);
					break;
				case "bounces":
					_bounces = int.Parse(lineSplit[1]);
					break;
				case "bossLevels":
					_bossLevelsBeaten.Clear();
					foreach (string level in lineSplit[1].Split('-'))
					{
						_bossLevelsBeaten.Add(level);
					}
					break;
				case "tutoFinished":
					_tutoFinished.Clear();
					foreach (string level in lineSplit[1].Split('-'))
					{
						_tutoFinished.Add(level);
					}
					break;
				default:
					break;
			}
		}

	}

	public void Save()
	{
		string saveString = "";

		saveString += "bestScore:" + _bestScore + "\n";
		saveString += "gamesPlayed:" + _gamesPlayed + "\n";
		saveString += "coins:" + _coins + "\n";
        saveString += "launchesAllowed:" + _launchesAllowed + "\n"; ;
        saveString += "viewRange:" + _viewRange + "\n"; ;
		saveString += "bounces:" + _bounces + "\n";
		saveString += "bossLevels:" + String.Join("-", _bossLevelsBeaten.ToArray()) + "\n";
		saveString += "bossLevels:" + String.Join("-", _tutoFinished.ToArray()) + "\n";

		Directory.GetParent(Application.persistentDataPath).Create();
		StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Save.dat");
		writer.Write(saveString);
		writer.Close();
	}

	public bool CanBuy(int value)
	{
		return _coins >= value;
	}
}
