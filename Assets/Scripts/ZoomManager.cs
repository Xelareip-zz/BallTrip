using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomManager : MonoBehaviour
{
	private static ZoomManager instance;
	public static ZoomManager Instance
	{
		get
		{
			return instance;
		}
	}
	public Vector2 margins = new Vector2(3, 2);
	
	public float pinchPreviousDist;

	public Vector3 nullVect = new Vector3(-10000, -10000);

	public Transform ballTransform;

	public float scrollSpeed;
	public float minSize;
	public float pinchSpeed;
	public float zoomLevel;
	public float targetZoom;
	public float zoomSpeed;

	public float targetBallPosition;
	public float snapSpeed;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		float tempSpeed = zoomSpeed;
		if (InfiniteGameManager.Instance.GetMode() == LAUNCH_MODE.FOLLOW)
		{
			tempSpeed /= 4.0f;
		}
		zoomLevel += (targetZoom - zoomLevel) * tempSpeed * Time.deltaTime;
		if (ballTransform == null || TutoManager.Instance.GetCanDragCamera() == false || InfiniteGameManager.Instance.gameIsOver)
		{
			return;
		}
		
		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			targetZoom += Input.GetAxis("Mouse ScrollWheel") * scrollSpeed;
		}
        if (Input.touchCount == 2)
		{
			float pinchDist = (Input.GetTouch(0).position - Input.GetTouch(1).position).magnitude;
            if (pinchPreviousDist > 0)
			{
				targetZoom += (pinchDist - pinchPreviousDist) * pinchSpeed;
			}
			pinchPreviousDist = pinchDist;
		}
		else
		{
			pinchPreviousDist = -1;
		}
		targetZoom = Mathf.Clamp01(targetZoom);
		Camera.main.orthographicSize = minSize + (1.0f - zoomLevel) * (GetMaxCameraHeight() - minSize);
		KeepCameraCentered();
	}

	void KeepCameraCentered()
	{
		Bounds globalBounds = InfiniteLevelsManager.Instance.GetGlobalBounds();
		float minY = globalBounds.center.y - globalBounds.extents.y;

		float camY = Ball.Instance.transform.position.y + Camera.main.orthographicSize - Camera.main.orthographicSize * 2.0f * targetBallPosition;
        Vector3 targetPos = new Vector3(Ball.Instance.transform.position.x, camY, transform.position.z);

		//float speed = (targetPos - transform.position).magnitude * snapSpeed * Time.deltaTime;

        //targetPos.y = Mathf.Min(targetPos.y, minY + Camera.main.orthographicSize);
		/*
		float targetSpeed = ((targetPos - transform.position) * maxSpeed).magnitude;
		snapSpeed += (targetSpeed - snapSpeed) * snapAcceleration * Time.deltaTime;
		*/
        transform.position += (targetPos - transform.position) * snapSpeed * Time.deltaTime;
    }

	void KeepCameraInBounds()
	{
		Bounds globalBounds = InfiniteLevelsManager.Instance.GetGlobalBounds();
		Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 40, GetMaxCameraHeight());

		globalBounds.Expand(new Vector3(2.0f * (margins.x - Camera.main.orthographicSize * Camera.main.aspect),
										2.0f * (margins.y - Camera.main.orthographicSize)));
		globalBounds.extents = new Vector3(Mathf.Max(0, globalBounds.extents.x), Mathf.Max(0, globalBounds.extents.y));
		Vector3 camPos = globalBounds.ClosestPoint(transform.position);
		camPos.z = transform.position.z;


		transform.position += (camPos - transform.position) * Time.deltaTime * 0.8f;
    }

	float GetMaxCameraHeight()
	{
		Bounds globalBounds = InfiniteLevelsManager.Instance.GetGlobalBounds();
		return globalBounds.extents.y + margins.y;
		Bounds firstLevelBounds = InfiniteLevelsManager.Instance.GetLevel(InfiniteLevelsManager.Instance.GetFirstLevelNumber()).GetCurrentBounds();
		Bounds lastLevelBounds = InfiniteLevelsManager.Instance.GetLevel(InfiniteLevelsManager.Instance.currentLevel).GetCurrentBounds();

		float yMin = firstLevelBounds.center.y - firstLevelBounds.extents.y;
		float yMax = lastLevelBounds.center.y + lastLevelBounds.extents.y;

		return yMax - yMin;
	}
}
