using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchManager : MonoBehaviour
{
	public int touchId = -1;
	public Vector3 dragOrigin;
	public Vector3 currentDrag;

	public Vector3 nullVect = new Vector3(-10000, -10000);

	public bool canShoot = false;
	public bool readyToShoot = false;

	public Color canShootColor;
	public Color cannotShootColor;

	public GameObject startDragUI;

	void Update()
	{
		Ball.Instance.launchDirectionScript.gameObject.SetActive(readyToShoot);
		Ball.Instance.launchDirectionScript.SetColor(canShoot ? canShootColor : cannotShootColor);
		Vector3 newPos = (dragOrigin - new Vector3(Screen.width, Screen.height) / 2.0f) * XUtils.ScreenCamRatio() + transform.position;
        startDragUI.transform.position = new Vector3(newPos.x, newPos.y, startDragUI.transform.position.z);
		startDragUI.SetActive(InfiniteGameManager.Instance.GetMode() == LAUNCH_MODE.LAUNCH && touchId >= 0);
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
			else
			{
				readyToShoot = false;
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

		Vector3 drag = (dragOrigin - currentDrag);// * XUtils.ScreenCamRatio();
		float angle = Quaternion.FromToRotation(Vector3.up, drag).eulerAngles.z;
		float newAngle = TutoManager.Instance.GetAngleModifier(angle);
		Ball.Instance.launchDirection = Quaternion.AngleAxis(newAngle, Vector3.forward) * Vector3.up;
		canShoot = TutoManager.Instance.GetCanShootAngle(newAngle);
		readyToShoot = drag.magnitude >= 100.0f;
		canShoot &= readyToShoot;
		Ball.Instance.launchDirectionScript.transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
		ZoomManager.Instance.targetZoom = 1.0f - Mathf.Clamp01((drag.magnitude - 100.0f) / 500.0f);
		if (shouldLaunch && canShoot)
		{
			readyToShoot = false;
			Ball.Instance.Launch();
		}
	}
}
