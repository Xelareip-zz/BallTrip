using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdaptCameraToAds : MonoBehaviour
{
	void Update ()
	{
		Rect viewportRect = new Rect(new Vector2(0, AdsManager.Instance.adsHeight), new Vector2(1.0f, 1.0f - AdsManager.Instance.adsHeight));
		Camera.main.rect = viewportRect;
	}
}
