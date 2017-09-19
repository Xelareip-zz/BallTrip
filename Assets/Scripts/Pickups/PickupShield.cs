using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupShield : PickupBase
{
	public float rotationSpeed;

	public override float GetDropWeight()
	{
		return Player.Instance.GetPUShieldChances();
	}

	void Update()
	{
		transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			new ModifierShield();
			Destroy(gameObject);
		}
	}
}
