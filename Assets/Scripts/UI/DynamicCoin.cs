using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCoin : MonoBehaviour
{
	public float speedAngle;
	public float initialSpeed;

	void Start ()
	{
		Rigidbody rig = GetComponent<Rigidbody>();
		
		rig.velocity = Quaternion.AngleAxis(Random.Range(-speedAngle, speedAngle), Vector3.forward) * Vector3.up * initialSpeed;
	}
}
