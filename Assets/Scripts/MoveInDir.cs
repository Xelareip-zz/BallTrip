using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveInDir : MonoBehaviour
{
	public Vector3 direction;
	public float speed;
	public float lifetime;


	// Update is called once per frame
	void Update ()
	{
		lifetime -= Time.deltaTime;
		if (lifetime <= 0.0f)
		{
			Destroy(gameObject);
		}
		transform.position += Time.deltaTime * speed * direction;
	}
}
