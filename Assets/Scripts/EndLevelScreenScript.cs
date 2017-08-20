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

	public Button continueButton;
	public bool rewardedVideoUsed;

	void Awake()
	{
		rewardedVideoUsed = false;
	}

	void OnEnable ()
	{
		levelText.text = (InfiniteLevelsManager.Instance.currentLevel - InfiniteLevelsManager.Instance.levels.Count).ToString();
		bestLevelText.text = Player.Instance.GetBestLevel().ToString();
		coinsText.text = InfiniteGameManager.Instance.currentCoins.ToString();
		continueButton.interactable = !rewardedVideoUsed;
	}

	public void Replay()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void GoToShop()
	{
		SceneManager.LoadScene("InfiniteLevelsMenu");
	}

	public void ShowAdContinue()
	{
		AdsManager.Instance.rewardedVideoCallback = ShowAdContinueFinished;
		AdsManager.Instance.ShowRewardedVideo();
		rewardedVideoUsed = true;
	}

	public void ShowAdContinueFinished(bool success)
	{
		if (success)
		{
			InfiniteGameManager.Instance.launchesLeft = Mathf.FloorToInt(Player.Instance.GetLaunches() / 2.0f) + 1;
			InfiniteGameManager.Instance.gameIsOver = false;
			InfiniteGameManager.Instance.SetLaunchMode(LAUNCH_MODE.LOOK);
			gameObject.SetActive(false);
		}
	}
}
