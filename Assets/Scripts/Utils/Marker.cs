using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
	public Color color;
	
	void Start()
	{
		Destroy(this);
	}


#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		Gizmos.color = color;
		Gizmos.DrawSphere(transform.position, 0.5f);
	}
#endif
}
