using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum BUYABLE
{
	LAUNCH,
	HEARTS,
	VIEW,
	PU_HEARTS,
	PU_ENERGY,
	PU_ROCKET,
	ENERGY
};

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
	public int _shieldLevel = 0;
	public int _bestLevel = 0;
	public int _hp = 10;

	public Dictionary<BUYABLE, int> _buyableLevels = new Dictionary<BUYABLE, int>();

	public List<string> _bossLevelsBeaten = new List<string>();
	public List<string> _tutoFinished = new List<string>();

	void Awake()
	{
		instance = this;
		DontDestroyOnLoad(gameObject);
		Reinit();
		Load();
	}

	public static string GetPath()
	{
		return Application.persistentDataPath + "/Save.dat";
    }

	public void Reinit()
	{
		_bestScore = 0;
		_gamesPlayed = 0;
		_coins = 15;
		_shieldLevel = 0;
		_bestLevel = 0;
		_hp = 10;

		_buyableLevels = new Dictionary<BUYABLE, int>();

		Array buyableValues = Enum.GetValues(typeof(BUYABLE));
        for (int idx = 0; idx < buyableValues.Length; ++idx)
		{
			_buyableLevels.Add((BUYABLE)buyableValues.GetValue(idx), 0);
		}

		_bossLevelsBeaten = new List<string>();
		_tutoFinished = new List<string>();
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

	public void SetShieldLevel(int level)
	{
		_shieldLevel = level;
		Save();
	}

	public void SetBestLevel(int level)
	{
		_bestLevel = Mathf.Max(_bestLevel, level);
		Save();
	}

	public int GetBestLevel()
	{
		return _bestLevel;
	}

	public int GetCoins()
	{
		return _coins;
	}

	public int GetLaunches()
	{
		return 1;// + GetBuyableLevel(BUYABLE.LAUNCH);
	}

	public int GetViewRange()
	{
		return 5;// 1 + GetBuyableLevel(BUYABLE.VIEW);
	}

	public int GetHearts()
	{
		return 1 + GetBuyableLevel(BUYABLE.HEARTS);
	}

	public int GetEnergy()
	{
		return 10 + GetBuyableLevel(BUYABLE.ENERGY) * 7;
	}

	public float GetPUHeartsChances()
	{
		return GetBuyableLevel(BUYABLE.PU_HEARTS) * 1.0f;
	}

	public float GetPUEnergyChances()
	{
		return GetBuyableLevel(BUYABLE.PU_ENERGY) * 7.0f;
	}

	public float GetPURocketChances()
	{
		return GetBuyableLevel(BUYABLE.PU_ROCKET) * 7.0f;
	}

	public int GetShieldLevel()
	{
		return _shieldLevel;
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

	public int GetBuyableLevel(BUYABLE buyable)
	{
		return _buyableLevels[buyable];
	}

	public void SetBuyableLevel(BUYABLE buyable, int level)
	{
		_buyableLevels[buyable] = level;
	}

	public int GetHp()
	{
		return _hp;
	}

	public void SetHp(int hp)
	{
		_hp = hp;
		Save();
	}

	public void ResetLevels()
	{
		_bossLevelsBeaten.Clear();
		Save();
	}

	private void Load()
	{
		if (File.Exists(GetPath()) == false)
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
				case "shieldLevel":
					_shieldLevel = int.Parse(lineSplit[1]);
					break;
				case "bestLevel":
					_bestLevel = int.Parse(lineSplit[1]);
					break;
				case "hp":
					_hp = int.Parse(lineSplit[1]);
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
				case "buyableLevels":
					_buyableLevels.Clear();
					Array buyableValues = Enum.GetValues(typeof(BUYABLE));
					for (int idx = 0; idx < buyableValues.Length; ++idx)
					{
						_buyableLevels.Add((BUYABLE)buyableValues.GetValue(idx), 0);
					}
					foreach (string buyable in lineSplit[1].Split('-'))
					{
						string[] buyableSplit = buyable.Split('~');
						_buyableLevels[(BUYABLE)Enum.Parse(typeof(BUYABLE), buyableSplit[0])] = int.Parse(buyableSplit[1]);
					}
					break;
				default:
					break;
			}
		}

	}

	public void Save()
	{
		List<string> buyableStrings = new List<string>();
		foreach (var kvp in _buyableLevels)
		{
			buyableStrings.Add(kvp.Key.ToString() + "~" + kvp.Value.ToString());
		}

		string saveString = "";

		saveString += "bestScore:" + _bestScore + "\n";
		saveString += "gamesPlayed:" + _gamesPlayed + "\n";
		saveString += "coins:" + _coins + "\n";
		saveString += "shieldLevel:" + _shieldLevel + "\n";
		saveString += "bestLevel:" + _bestLevel + "\n";
		saveString += "hp:" + _hp + "\n";
		saveString += "bossLevels:" + String.Join("-", _bossLevelsBeaten.ToArray()) + "\n";
		saveString += "tutoFinished:" + String.Join("-", _tutoFinished.ToArray()) + "\n";
		saveString += "buyableLevels:" + String.Join("-", buyableStrings.ToArray()) + "\n";

		Directory.GetParent(Application.persistentDataPath).Create();
		StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/Save.dat");
		writer.Write(saveString);
		writer.Close();
	}

	public bool BuyableUnlocked(BUYABLE buyable)
	{
		switch (buyable)
		{
			case BUYABLE.HEARTS:
			case BUYABLE.LAUNCH:
			case BUYABLE.VIEW:
			case BUYABLE.ENERGY:
				return true;
			case BUYABLE.PU_HEARTS:
				return GetLevelBeaten(BOSS_TYPE.HEART);
			case BUYABLE.PU_ENERGY:
				return GetLevelBeaten(BOSS_TYPE.ENERGY);
            case BUYABLE.PU_ROCKET:
				return GetLevelBeaten(BOSS_TYPE.ROCKET);
		}
		return false;
	}

	public bool CanBuy(int value)
	{
		return _coins >= value;
	}
}
