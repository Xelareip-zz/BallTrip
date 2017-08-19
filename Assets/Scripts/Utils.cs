using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XUtils
{
	public static float ScreenCamRatio()
	{
		return 2.0f * Camera.main.orthographicSize / (Screen.height * Camera.main.rect.height);
	}

	public static Bounds GetBounds(List<Transform> objs)
	{
		if (objs == null || objs.Count == 0)
		{
			return new Bounds(Vector3.zero, Vector3.zero);
		}

		float minX = Mathf.Infinity;
		float maxX = -Mathf.Infinity;
		float minY = Mathf.Infinity;
		float maxY = -Mathf.Infinity;
		float minZ = Mathf.Infinity;
		float maxZ = -Mathf.Infinity;

		Vector3[] points = new Vector3[8];

		foreach (Transform go in objs)
		{
			GetBoundsPointsNoAlloc(go, points);
			foreach (Vector3 v in points)
			{
				if (v.x < minX) minX = v.x;
				if (v.x > maxX) maxX = v.x;
				if (v.y < minY) minY = v.y;
				if (v.y > maxY) maxY = v.y;
				if (v.z < minZ) minZ = v.z;
				if (v.z > maxZ) maxZ = v.z;
			}
		}

		float sizeX = maxX - minX;
		float sizeY = maxY - minY;
		float sizeZ = maxZ - minZ;

		Vector3 center = new Vector3(minX + sizeX / 2.0f, minY + sizeY / 2.0f, minZ + sizeZ / 2.0f);

		return new Bounds(center, new Vector3(sizeX, sizeY, sizeZ));
	}

	public static void GetBoundsPointsNoAlloc(Transform tr, Vector3[] points)
	{
		if (points == null || points.Length < 8)
		{
			Debug.Log("Bad Array");
			return;
		}
		Collider mf = tr.GetComponent<Collider>();
		if (mf == null)
		{
			for (int i = 0; i < points.Length; i++)
				points[i] = tr.position;
			return;
		}

		Bounds bounds = mf.bounds;
		Vector3 v3Center = bounds.center;
		Vector3 v3ext = bounds.extents;

		points[0] = new Vector3(v3Center.x - v3ext.x, v3Center.y + v3ext.y, v3Center.z - v3ext.z);  // Front top left corner
		points[1] = new Vector3(v3Center.x + v3ext.x, v3Center.y + v3ext.y, v3Center.z - v3ext.z);  // Front top right corner
		points[2] = new Vector3(v3Center.x - v3ext.x, v3Center.y - v3ext.y, v3Center.z - v3ext.z);  // Front bottom left corner
		points[3] = new Vector3(v3Center.x + v3ext.x, v3Center.y - v3ext.y, v3Center.z - v3ext.z);  // Front bottom right corner
		points[4] = new Vector3(v3Center.x - v3ext.x, v3Center.y + v3ext.y, v3Center.z + v3ext.z);  // Back top left corner
		points[5] = new Vector3(v3Center.x + v3ext.x, v3Center.y + v3ext.y, v3Center.z + v3ext.z);  // Back top right corner
		points[6] = new Vector3(v3Center.x - v3ext.x, v3Center.y - v3ext.y, v3Center.z + v3ext.z);  // Back bottom left corner
		points[7] = new Vector3(v3Center.x + v3ext.x, v3Center.y - v3ext.y, v3Center.z + v3ext.z);  // Back bottom right corner
	}

	public static Bounds GetBounds(BoxCollider collider)
	{
		return new Bounds(collider.center, collider.size);
	}

	public static Bounds GetBounds(MeshCollider collider)
	{
		return collider.bounds;
	}
}
