using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshChanger : MonoBehaviour
{
	public MeshFilter filter;
	public MeshCollider collider;
	public Mesh mesh;
	void Start()
	{
		filter = GetComponent<MeshFilter>();
		collider = GetComponent<MeshCollider>();
		mesh = new Mesh();
		mesh.SetVertices(new List<Vector3>()
		{
			new Vector3(0, 0, 0),
			new Vector3(0, 1, 0),
			new Vector3(1, 1, 0),
			new Vector3(1, 0, 0),
			new Vector3(0, 0, 0),
			new Vector3(1, 1, 0),
		});
		mesh.SetTriangles(new int[] { 0, 1, 2, 3, 4, 5 }, 0);
		filter.mesh = mesh;
		collider.sharedMesh = filter.mesh;
	}
	
	void Update ()
	{
		List<Vector3> vertices = new List<Vector3>();
		mesh.GetVertices(vertices);
		vertices[2] = vertices[2] - Vector3.one * Time.deltaTime * 0.1f;
		mesh.SetVertices(vertices);
		filter.mesh = mesh;
		collider.sharedMesh = mesh;

    }
}
