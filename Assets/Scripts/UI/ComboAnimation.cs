using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboAnimation : MonoBehaviour
{
	public float duration;

	public Text comboText;

	private float startTime;

	void Awake()
	{
		startTime = float.MinValue;
	}

	public void Show()
	{
		startTime = Time.time;
	}

	void Update()
	{
		comboText.enabled = (Time.time - startTime) < duration;
		if (comboText.enabled)
		{
			comboText.text = "Combo x" + Ball.Instance.combo;
			Color color = comboText.color;
			color.a = (Time.time - startTime) / duration;
			comboText.color = color;
		}
	}
}
