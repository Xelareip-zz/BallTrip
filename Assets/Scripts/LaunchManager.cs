using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManager : MonoBehaviour
{
	public int touchId = -1;
	public Vector3 dragOrigin;
	public Vector3 currentDrag;

	public Vector3 nullVect = new Vector3(-10000, -10000);

	void Update()
	{
		if (Ball.Instance.ballRigidbody.velocity.magnitude != 0.0f || InfiniteGameManager.Instance.GetMode() == LAUNCH_MODE.LOOK)
		{
			touchId = -1;
			return;
		}

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

			dragOrigin = position;
			return;
		}

		bool shouldLaunch = false;

		if (Input.touchCount != 0)
		{
			Touch touch = Input.GetTouch(touchId);
			currentDrag = touch.position;
			shouldLaunch = touch.phase == TouchPhase.Ended;
			if (shouldLaunch)
			{
				touchId = -1;
			}
		}
		else if (Input.GetMouseButton(0))
		{
			currentDrag = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			currentDrag = Input.mousePosition;
			shouldLaunch = true;
		}
		// Touch cancelled, loss of focus, etc...
		else
		{
			Ball.Instance.launchDirection = Vector3.zero;
			touchId = -1;
			return;
		}

		Vector3 drag = (dragOrigin - currentDrag) * XUtils.ScreenCamRatio();
		Ball.Instance.launchDirection = drag.normalized;

		if (shouldLaunch)
		{
			Ball.Instance.Launch();
		}
	}
}
