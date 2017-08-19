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

	public int currentCoins;
	private int currentCoinsUI;

	public int launchesLeft;

	void Awake ()
	{
		instance = this;
		launchesLeft = Player.Instance._launchesAllowed;
		currentCoins = 0;
		SetCoinsText();
		gameIsOver = false;
	}

	void Start()
	{
		TutoManager.Instance.StartTuto("TutoFirstLaunch");
		if (Player.Instance.GetTutoFinished("TutoFirstLaunch") && Player.Instance.GetShieldLevel() == 0)
		{
			TutoManager.Instance.StartTuto("TutoSecondLaunch");
		}
		if (Player.Instance.GetTutoFinished("TutoSecondLaunch"))
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
		if (mode == LAUNCH_MODE.LOOK && launchesLeft == 0)
		{
			EndGame();
		}
		launchMode = mode;
		lookModeUI.SetActive(launchMode == LAUNCH_MODE.LOOK);
	}

	public void EndGame()
	{
		gameIsOver = true;
		launchMode = LAUNCH_MODE.LOOK;
		lookModeUI.SetActive(false);
		if (Player.Instance.GetTutoFinished("TutoSecondLaunch") == false)
		{
			endLevelScreen.GoToShop();
        }
		else
		{
			endLevelScreen.gameObject.SetActive(true);
		}
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
			GameObject newUI = Instantiate<GameObject>(uiCoinsWon);
			newUI.transform.SetParent(Camera.main.transform);
			newUI.transform.position = Ball.Instance.transform.position + Vector3.back;
			newUI.GetComponent<GoTo>().finished += () => AddCoinsApply(coinsToAdd);
			newUI.SetActive(true);
		}
		else
		{
			AddCoinsApply(coinsToAdd);
        }
	}

	private void AddCoinsApply(int coins)
	{
		SetCoinsText();
	}

	private void SetCoinsText()
	{
		coinsText.text = "" + currentCoins;
	}

	private void SetBouncesText()
	{
		heartsText.text = "" + Mathf.Max(Ball.Instance.currentHeartCountUI, 0);
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
		newUI.transform.position = Ball.Instance.transform.position + Vector3.back;
		newUI.GetComponent<GoTo>().finished += FreeLaunchIncreaseApply;
		newUI.SetActive(true);
	}

	private void FreeLaunchIncreaseApply()
	{
		++freeLaunchGaugeValue;
	}
}
