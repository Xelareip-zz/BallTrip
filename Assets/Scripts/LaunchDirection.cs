using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchDirection : MonoBehaviour
{
	public float distanceFromRoot;
	public Material mainMaterial;
	public GameObject mainRenderer;

	void Awake()
	{
		distanceFromRoot = mainRenderer.transform.localPosition.y - mainRenderer.transform.localScale.y / 2.0f;
    }

	public void SetColor(Color color)
	{
		mainMaterial.color = color;
	}

	void Update()
	{
		float length = (1.0f - ZoomManager.Instance.zoomLevel) * Camera.main.orthographicSize + 10;
        mainMaterial.SetFloat("repeatsY", length);

		mainRenderer.transform.localScale = new Vector3(1.0f, length, 1.0f);
		mainRenderer.transform.localPosition = new Vector3(0.0f, distanceFromRoot + length / 2.0f, 0.0f);
    }
}
