using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndLevelScreenScript : MonoBehaviour
{
	public Text levelText;
	public Text bestLevelText;
	public Text coinsText;

	void Start ()
	{
		levelText.text = (InfiniteLevelsManager.Instance.currentLevel - InfiniteLevelsManager.Instance.levels.Count).ToString();
		bestLevelText.text = Player.Instance.GetBestLevel().ToString();
		coinsText.text = InfiniteGameManager.Instance.currentCoins.ToString();
	}

	public void Replay()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void GoToShop()
	{
		SceneManager.LoadScene("InfiniteLevelsMenu");
	}
}
