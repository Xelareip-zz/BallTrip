using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptCameraToAds : MonoBehaviour
{
	void Update ()
	{
		Rect viewportRect = new Rect(0.0f, AdsManager.Instance.bannersHeight, 1.0f, 1.0f - AdsManager.Instance.bannersHeight);
		Camera.main.rect = viewportRect;
	}
}
