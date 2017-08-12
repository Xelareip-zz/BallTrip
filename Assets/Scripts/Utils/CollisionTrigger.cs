using UnityEngine;
using System;

public class CollisionTrigger : MonoBehaviour
{
	public event Action<Collider> collisionEnter;
	public event Action<Collider> collisionExit;

	void OnTriggerEnter(Collider coll)
	{
		if (collisionEnter != null)
		{
			collisionEnter(coll);
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if (collisionExit != null)
		{
			collisionExit(coll);
		}
	}
}
