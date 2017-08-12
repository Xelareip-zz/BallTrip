using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUI : MonoBehaviour
{
	public RectTransform target;

	void Start()
	{
		float z = transform.position.z;

		Vector3[] corners = new Vector3[4];
		target.GetWorldCorners(corners);
		Vector3 result = Vector3.zero;
		for (int cornerIdx = 0; cornerIdx < 4; ++cornerIdx)
		{
			result += corners[cornerIdx];
		}

		result /= 4;
		transform.position = new Vector3(result.x, result.y, z);
	}
}
