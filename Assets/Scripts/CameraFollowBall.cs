using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowBall : MonoBehaviour
{
	public Transform ballTransform;

	void FixedUpdate()
	{
		if (ballTransform == null || InfiniteGameManager.Instance.GetMode() != LAUNCH_MODE.FOLLOW)
		{
			return;
		}

		Vector2 cameraPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 ballPos = new Vector2(ballTransform.position.x, ballTransform.position.y);
		Vector2 difference = ballPos - cameraPos;

		Vector2 newPos = cameraPos + difference * 1.5f * Mathf.Min(Time.fixedDeltaTime	, 1.0f);

		transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
	}
}
