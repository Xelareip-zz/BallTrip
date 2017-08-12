using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelDisplayer : MonoBehaviour
{
	public GameObject buttonPrefab;

	public RectTransform viewTransform;

	public int columns;
	public int maxItems;

	public Vector4 padding;

	public List<RectTransform> currentButtons;

	public void SetUIForLevels(List<string> levelNames)
	{
		currentButtons = new List<RectTransform>();
		for (int levelIdx = 0; levelIdx < levelNames.Count; ++levelIdx)
		{
			GameObject newButton = Instantiate(buttonPrefab, viewTransform);
			newButton.GetComponentInChildren<Text>().text = "" + levelIdx;
			Button newButtonScript = newButton.GetComponent<Button>();
			string sceneName = levelNames[levelIdx];
			newButtonScript.onClick.AddListener(() =>
			{
                SceneManager.LoadScene(sceneName);
			});
			currentButtons.Add(newButton.transform as RectTransform);
		}

		UpdateLayout();
	}

	private void UpdateLayout()
	{
		float buttonSpace = (viewTransform.rect.width - padding.x - padding.y) / columns;
		int currentColumn = 0;
		int currentRow = 0;
		for (int buttonIdx = 0; buttonIdx < currentButtons.Count; ++buttonIdx)
		{
			currentColumn = buttonIdx % columns;
			currentRow = (buttonIdx - currentColumn) / columns;
			RectTransform current = currentButtons[buttonIdx];

			current.pivot = new Vector2(0.5f, 0.5f);
			current.anchorMin = new Vector2(0, 1);
			current.anchorMax = new Vector2(0, 1);
			current.anchoredPosition = new Vector2(padding.x + buttonSpace / 2 + buttonSpace * currentColumn, - (padding.z + padding.w / 2 + padding.w * currentRow));
		}

		viewTransform.sizeDelta = new Vector2(viewTransform.sizeDelta.x, padding.z + padding.w / 2 + padding.w * (currentRow + 1));
	}

	void Update()
	{
		UpdateLayout();
	}
}
