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
		AnalyticsSender.Instance.Send("end_screen", new Dictionary<string, object>()
		{
			{ "level_reached",  InfiniteLevelsManager.Instance.currentLevel - InfiniteLevelsManager.Instance.levels.Count},
			{ "initial_best", InfiniteLevelsManager.Instance.initialBestLevel},
			{ "coins", InfiniteGameManager.Instance.currentCoins}
		});
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
			//InfiniteGameManager.Instance.launchesLeft = Mathf.FloorToInt(Player.Instance.GetLaunches() / 2.0f) + 1;
			Ball.Instance.HeartIncrease(Mathf.FloorToInt(Player.Instance.GetHearts() / 2.0f) + 1);
			InfiniteGameManager.Instance.gameIsOver = false;
			InfiniteGameManager.Instance.SetLaunchMode(LAUNCH_MODE.LAUNCH);
			gameObject.SetActive(false);
		}
	}
}
