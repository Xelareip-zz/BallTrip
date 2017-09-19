using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public enum LAUNCH_MODE
{
	LAUNCH,
	FOLLOW,
	LOOK
}

public class InfiniteGameManager : MonoBehaviour
{
	private static InfiniteGameManager instance;
	public static InfiniteGameManager Instance
	{
		get
		{
			return instance;
		}
	}

	public LAUNCH_MODE launchMode = LAUNCH_MODE.LOOK;

	public GameObject lookModeUI;
	public Text coinsText;
	public Text heartsText;
	public Text launchesText;

	public CameraShaker cameraShaker;

	public List<Color> themeColors;
	public float currentColorCursor;
	public float currentColorCursorProgress;
	public Color currentColor;
	public Material wallMaterial;
	
	public RectTransform freeLaunchGaugeTransform;
	public GameObject uiFreeLaunch;
	public int freeLaunchGaugeMax;
	public int freeLaunchGaugeValue;

	public bool gameIsOver;
	public EndLevelScreenScript endLevelScreen;

	public GameObject uiCoinsWon;
	public GameObject dynamicCoinPrefab;

	public int currentCoins;
	public int currentCoinsUI;

	public int launchesLeft;

	void Awake ()
	{
		instance = this;
		launchesLeft = Player.Instance.GetLaunches();
		currentCoins = 0;
		SetCoinsText();
		gameIsOver = false;
		SetHpGauge(1.0f);
	}

	void Start()
	{
		if (TutoManager.Instance.StartTuto("TutoFirstLaunch") == false)
		{
			TutoManager.Instance.StartTuto("TutoFirstDrag");
		}
		if (TutoManager.Instance.GetCanDragCamera() == false)
		{
			SetLaunchMode(LAUNCH_MODE.LAUNCH);
		}
	}
	
	void Update()
	{
		SetBouncesText();
		SetLaunchesText();
		SetHpGauge();
		if (currentColorCursorProgress < currentColorCursor)
		{
			currentColorCursorProgress += 0.1f * Time.deltaTime;
		}
		UpdateBackgroundColor();
	}

	public void UpdateBackgroundColor()
	{
		Color previousColor = themeColors[Mathf.FloorToInt(currentColorCursorProgress) % themeColors.Count];
		Color nextColor = themeColors[Mathf.FloorToInt(currentColorCursorProgress + 1) % themeColors.Count];
		float colorProgression = currentColorCursorProgress - Mathf.FloorToInt(currentColorCursorProgress);
		Color newColor = previousColor * (1.0f - colorProgression) + nextColor * colorProgression;
		currentColor = newColor;
		wallMaterial.color = currentColor;
	}

	public void StartLaunchMode()
	{
		SetLaunchMode(LAUNCH_MODE.LAUNCH);
		float yDiff = Camera.main.orthographicSize * 0.8f;
		float xDiff = yDiff * Camera.main.rect.size.x / Camera.main.rect.size.y;

		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, Ball.Instance.transform.position.x - xDiff, Ball.Instance.transform.position.x + xDiff),
			Mathf.Clamp(transform.position.y, Ball.Instance.transform.position.y - yDiff, Ball.Instance.transform.position.y + yDiff),
			transform.position.z
			);
	}

	public void SetLaunchMode(LAUNCH_MODE mode)
	{
		if (mode == LAUNCH_MODE.LOOK && Ball.Instance.currentHeartCount == 0)
		{
			EndGame();
		}
		else
		{
			launchMode = mode;
			lookModeUI.SetActive(launchMode == LAUNCH_MODE.LOOK);
		}
	}

	public void EndGame()
	{
		TutoManager.Instance.StartTuto("TutoFirstDeath");
		StartCoroutine(EndGameEachFrame());
	}

	private IEnumerator EndGameEachFrame()
	{
		while (TutoManager.Instance.GetCanEndGame() == false)
		{
			yield return new WaitForEndOfFrame();
		}
		EndGameApply();
		yield return null;
	}

	private void EndGameApply()
	{
		gameIsOver = true;
		launchMode = LAUNCH_MODE.LOOK;
		lookModeUI.SetActive(false);
		endLevelScreen.gameObject.SetActive(true);
		/*
		if (Player.Instance.GetTutoFinished("TutoSecondLaunch") == false)
		{
			endLevelScreen.GoToShop();
		}
		else
		{
			endLevelScreen.gameObject.SetActive(true);
		}*/
	}

	public void GoToMainMenu()
	{
		SceneManager.LoadScene("InfiniteLevelsMenu");
	}

	public LAUNCH_MODE GetMode()
	{
		return launchMode;
	}

	public void AddCoins(int coinsToAdd, bool animation = true)
	{
		Player.Instance.AddCoins(coinsToAdd);
		currentCoins += coinsToAdd;
		if (animation)
		{
			for (int coinIdx = 0; coinIdx < coinsToAdd; ++coinIdx)
			{
				GameObject newCoin = Instantiate<GameObject>(dynamicCoinPrefab, Ball.Instance.transform.position, Quaternion.identity, InfiniteLevelsManager.Instance.levelsObj[1].transform);
			}
			//GameObject newUI = Instantiate<GameObject>(uiCoinsWon);
			//newUI.transform.SetParent(Camera.main.transform);
			//newUI.transform.position = Ball.Instance.transform.position + Vector3.back;
			//newUI.GetComponent<GoTo>().finished += () => AddCoinsApply(coinsToAdd);
			//newUI.SetActive(true);
		}
		AddCoinsApply(coinsToAdd);
	}

	private void AddCoinsApply(int coins)
	{
		SetCoinsText();
	}

	private void SetCoinsText()
	{
		coinsText.text = currentCoins.ToString();
	}

	private void SetBouncesText()
	{
		heartsText.text = Mathf.Max(Ball.Instance.currentHeartCountUI, 0).ToString();
	}

	private void SetLaunchesText()
	{
		launchesText.text = Mathf.RoundToInt(Ball.Instance.GetHp()).ToString();
	}

	private void SetHpGauge(float percentage = -1.0f)
	{
		float maxWidth = (freeLaunchGaugeTransform.parent as RectTransform).rect.width;
		if (percentage >= 0.0f && percentage <= 1.0f)
		{
			freeLaunchGaugeTransform.offsetMax = new Vector2(maxWidth * percentage, 0);
			return;
		}
		float stepSize = maxWidth / 2.0f;

		float targetX = maxWidth * Ball.Instance.GetHp() / (float)Player.Instance.GetEnergy();

		float newX;
		if (targetX >= freeLaunchGaugeTransform.offsetMax.x)
		{
			newX = Mathf.Min(freeLaunchGaugeTransform.offsetMax.x + stepSize * Time.deltaTime, targetX);
		}
		else
		{
			newX = Mathf.Max(freeLaunchGaugeTransform.offsetMax.x - stepSize * Time.deltaTime, targetX);
		}

		freeLaunchGaugeTransform.offsetMax = new Vector2(Mathf.Clamp(newX, 0, maxWidth), 0);
		/*
		if (newX >= maxWidth)
		{
			freeLaunchGaugeValue -= freeLaunchGaugeMax;
			++launchesLeft;
		}*/
	}
	/*
	private void SetFreeLaunchesText()
	{
		float maxWidth = (freeLaunchGaugeTransform.parent as RectTransform).rect.width;
		float stepSize = 2.0f * maxWidth / freeLaunchGaugeMax;

		float targetX = maxWidth * freeLaunchGaugeValue / (float)freeLaunchGaugeMax;

		float newX;
		if (targetX >= freeLaunchGaugeTransform.offsetMax.x)
		{
			newX = Mathf.Min(freeLaunchGaugeTransform.offsetMax.x + stepSize * Time.deltaTime, targetX);
		}
		else
		{
			newX = Mathf.Max(freeLaunchGaugeTransform.offsetMax.x - stepSize * Time.deltaTime, targetX);
		}

		freeLaunchGaugeTransform.offsetMax = new Vector2(Mathf.Clamp(newX, 0, maxWidth), 0);

		if (newX >= maxWidth)
		{
			freeLaunchGaugeValue -= freeLaunchGaugeMax;
			++launchesLeft;
		}
	}*/

	public void FreeLaunchIncrease()
	{
		GameObject newUI = Instantiate<GameObject>(uiFreeLaunch);
		newUI.transform.SetParent(Camera.main.transform);
		newUI.transform.position = Ball.Instance.transform.position + Vector3.back;
		newUI.GetComponent<GoTo>().finished += FreeLaunchIncreaseApply;
		newUI.SetActive(true);
	}

	private void FreeLaunchIncreaseApply()
	{
		++freeLaunchGaugeValue;
	}

	public int GetFreeHeartDistance(int currentLevel)
	{
		return Mathf.FloorToInt(currentLevel / 20.0f) + 2;
	}
}
