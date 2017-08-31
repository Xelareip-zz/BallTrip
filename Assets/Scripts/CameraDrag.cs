using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDrag : MonoBehaviour
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

		if (Ball.Instance.ballRigidbody.velocity.magnitude != 0.0f || InfiniteGameManager.Instance.GetMode() != LAUNCH_MODE.LOOK)
		{
			touchId = -1;
            dragOrigin = nullVect;
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

		Vector3 position = nullVect;
		if (touchId == -1)
		{
			if (Input.touchCount != 0)
			{
				Touch touch = Input.GetTouch(0);
				if (touch.phase == TouchPhase.Began)
				{
					touchId = touch.fingerId;
					position = touch.position;
				}
			}
			else if (Input.GetMouseButtonDown(0))
			{
				touchId = 0;
				position = Input.mousePosition;
			}
			else if (Input.GetMouseButtonUp(0))
			{
				touchId = 0;
				position = nullVect;
			}

			dragOrigin = position;
		}
		if (dragOrigin == nullVect)
		{
			return;
		}


		if (Input.touchCount != 0)
		{
			Touch touch = Input.GetTouch(touchId);
			currentDrag = touch.position;
		}
		else if (Input.GetMouseButton(0))
		{
			currentDrag = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			currentDrag = Input.mousePosition;
		}
		// Touch cancelled, loss of focus, etc...
		else
		{
			Ball.Instance.launchDirection = Vector3.zero;
			touchId = -1;
			return;
		}

		Vector3 drag = (dragOrigin - currentDrag) * XUtils.ScreenCamRatio();
		Vector3 newPos = transform.position + drag;
		dragOrigin = currentDrag;

		transform.position = newPos;
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
        //transform.position = new Vector3(camPos.x, camPos.y, transform.position.z);



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
