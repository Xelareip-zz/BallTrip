using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
	public Text scoreText;
	public Text bouncesText;
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

	public GameObject uiCoinWon;

	private int score;

	public int launchesLeft;

	void Awake ()
	{
		instance = this;
		launchesLeft = Player.Instance._launchesAllowed;
		score = 0;
		SetScoreText();
	}

	void Start()
	{
		TutoManager.Instance.StartTuto("TutoFirstLaunch");
	}
	
	void Update()
	{
		SetBouncesText();
		SetLaunchesText();
		SetFreeLaunchesText();
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
	}

	public void SetLaunchMode(LAUNCH_MODE mode)
	{
		if (mode == LAUNCH_MODE.LOOK && launchesLeft == 0)
		{
			EndGame();
		}
		launchMode = mode;
		lookModeUI.SetActive(launchMode == LAUNCH_MODE.LOOK);
	}

	public void EndGame()
	{
		SceneManager.LoadScene("InfiniteLevelsMenu");
	}

	public LAUNCH_MODE GetMode()
	{
		return launchMode;
	}

	public void AddScore(int scoreToAdd)
	{
		GameObject newUI = Instantiate<GameObject>(uiCoinWon);
		newUI.transform.SetParent(Camera.main.transform);
		newUI.transform.position = Ball.Instance.transform.position;
		newUI.GetComponent<GoTo>().finished += () => AddCoinsApply(scoreToAdd);
		newUI.SetActive(true);
	}

	private void SetScoreText()
	{
		scoreText.text = "" + score;
	}

	private void SetBouncesText()
	{
		bouncesText.text = "" + Ball.Instance.currentCollisionCount;
	}

	private void SetLaunchesText()
	{
		launchesText.text = "" + launchesLeft;
	}

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
	}

	public void FreeLaunchIncrease()
	{
		GameObject newUI = Instantiate<GameObject>(uiFreeLaunch);
		newUI.transform.SetParent(Camera.main.transform);
		newUI.transform.position = Ball.Instance.transform.position;
		newUI.GetComponent<GoTo>().finished += FreeLaunchIncreaseApply;
		newUI.SetActive(true);
	}

	private void FreeLaunchIncreaseApply()
	{
		++freeLaunchGaugeValue;
	}

	private void AddCoinsApply(int coins)
	{
		Player.Instance.AddCoins(coins);
		score += coins;
		SetScoreText();
	}
}
