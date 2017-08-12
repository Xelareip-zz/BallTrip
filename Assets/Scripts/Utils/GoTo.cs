using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTo : MonoBehaviour
{
	public Transform target;
	public float duration;
	public event System.Action finished;

	void Update()
	{
		transform.position = transform.position * (1 - Time.deltaTime / duration) + target.position * (Time.deltaTime / duration);
		duration = Mathf.Max(0, duration - Time.deltaTime);
		if (duration <= 0)
		{
			if (finished != null)
			{
				finished();
			}
			Destroy(gameObject);
		}
	}
}
