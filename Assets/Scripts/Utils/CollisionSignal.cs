using UnityEngine;
using System;

public class CollisionSignal : MonoBehaviour
{
	public event Action<CollisionSignal, Collision> collisionEnter;
	public event Action<CollisionSignal, Collision> collisionExit;

	void OnCollisionEnter(Collision coll)
	{
		if (collisionEnter != null)
		{
			collisionEnter(this, coll);
		}
	}

	void OnCollisionExit(Collision coll)
	{
		if (collisionExit != null)
		{
			collisionExit(this, coll);
		}
	}
}
