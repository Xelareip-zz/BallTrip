using UnityEngine;
using System;

public class TriggerSignal : MonoBehaviour
{
	public event Action<TriggerSignal, Collider> collisionStay;
	public event Action<Collider> collisionEnter;
	public event Action<Collider> collisionExit;

	void OnTriggerEnter(Collider coll)
	{
		if (collisionEnter != null)
		{
			collisionEnter(coll);
		}
	}

	void OnTriggerStay(Collider coll)
	{
		if (collisionStay != null)
		{
			collisionStay(this, coll);
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
