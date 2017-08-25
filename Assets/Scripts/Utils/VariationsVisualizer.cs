using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class VariationsVisualizer : MonoBehaviour
{
	public static string selectedGUID;

	public Camera cam;
	public RenderTexture renderTexture;
	public GameObject instance;
	public Vector2 variationPosition;
	public InfiniteLevel level;

	void Awake()
	{
		renderTexture = new RenderTexture(128, 256, 0);
		cam.targetTexture = renderTexture;
	}

	public void ManualUpdate()
	{
		if (level == null)
		{
			return;
		}

		transform.position = new Vector3(variationPosition.x * 50, variationPosition.y * 50, 0);

		IGUIDIdentified[] guids = GetComponentsInChildren<IGUIDIdentified>(true);
		var variations = level.variationsLevels[(int)variationPosition.x][(int)variationPosition.y];
        foreach (IGUIDIdentified guidObj in guids)
		{
			if (guidObj.GetGUID() == selectedGUID)
			{
				(guidObj as MonoBehaviour).gameObject.SetActive((guidObj as MonoBehaviour).gameObject.activeSelf == false);
            }
			else
			{
				(guidObj as MonoBehaviour).gameObject.SetActive(variations.Contains(guidObj.GetGUID()));
			}
		}
	}

	public void SetLevel(GameObject prefab)
	{
		if (level != null)
		{
			DestroyImmediate(level.gameObject);
		}

		GameObject go = Instantiate<GameObject>(prefab);
		go.transform.SetParent(transform);
		go.transform.localPosition = new Vector3(0, 0, 5);
		level = prefab.GetComponent<InfiniteLevel>();
	}
}
