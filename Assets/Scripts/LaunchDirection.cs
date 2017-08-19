using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchDirection : MonoBehaviour
{
	public float initialRatio;
	public Material mainMaterial;
	public GameObject mainRenderer;
	
	void Start ()
	{
		initialRatio = mainRenderer.transform.lossyScale.x / mainRenderer.transform.lossyScale.y;
	}

	public void SetLength(float length)
	{
		mainRenderer.transform.localScale = new Vector3(length * initialRatio, length, 1.0f);
	}

	public void SetColor(Color color)
	{
		mainMaterial.color = color;
	}
}
