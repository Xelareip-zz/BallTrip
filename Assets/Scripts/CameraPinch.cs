using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPinch : MonoBehaviour
{
	public Vector2 margins = new Vector2(3, 2);

	public int touchId = -1;
	public Vector3 dragOrigin;
	public Vector3 currentDrag;

	public float pinchPreviousDist;

	public Vector3 nullVect = new Vector3(-10000, -10000);

	public Transform ballTransform;

	void Update()
	{
		if (ballTransform == null || TutoManager.Instance.GetCanDragCamera() == false || InfiniteGameManager.Instance.gameIsOver)
		{
			return;
		}
	
		if (Input.touchCount > 1)
		{
			touchId = -1;
			dragOrigin = nullVect;
        }

		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 5;
		}
        if (Input.touchCount == 2)
		{
			float pinchDist = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
            if (pinchPreviousDist > 0)
			{
				Camera.main.orthographicSize -= (pinchDist - pinchPreviousDist) * 0.5f;
			}
			pinchPreviousDist = pinchDist;
		}
		else
		{
			pinchPreviousDist = -1;
		}

		KeepCameraInBounds();
	}

	void KeepCameraInBounds()
	{

		Bounds globalBounds = InfiniteLevelsManager.Instance.GetGlobalBounds();
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 40, globalBounds.extents.y + margins.y);

		globalBounds.Expand(new Vector3(2.0f * (margins.x - Camera.main.orthographicSize * Camera.main.aspect),
										2.0f * (margins.y - Camera.main.orthographicSize)));
		globalBounds.extents = new Vector3(Mathf.Max(0, globalBounds.extents.x), Mathf.Max(0, globalBounds.extents.y));
		Vector3 camPos = globalBounds.ClosestPoint(transform.position);
		camPos.z = transform.position.z;


		transform.position += (camPos - transform.position) * Time.deltaTime * 0.8f;
    }

	float GetMaxCameraHeight()
	{
		Bounds firstLevelBounds = InfiniteLevelsManager.Instance.GetLevel(InfiniteLevelsManager.Instance.GetFirstLevelNumber()).GetCurrentBounds();
		Bounds lastLevelBounds = InfiniteLevelsManager.Instance.GetLevel(InfiniteLevelsManager.Instance.currentLevel).GetCurrentBounds();

		float yMin = firstLevelBounds.center.y - firstLevelBounds.extents.y;
		float yMax = lastLevelBounds.center.y + lastLevelBounds.extents.y;

		return yMax - yMin;
	}
}
