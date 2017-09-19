using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldManager : MonoBehaviour
{
	public int maxShields;

	public List<GameObject> shields;
	public float rotationSpeed;

	void Update ()
	{
		int shieldCount = 0;
		for (int modIdx = 0; modIdx < Ball.Instance.modifiers.Count; ++modIdx)
		{
			if (Ball.Instance.modifiers[modIdx] is ModifierShield)
			{
				if (shieldCount < maxShields)
				{
					++shieldCount;
				}
				else
				{
					Ball.Instance.RemoveModifier(Ball.Instance.modifiers[modIdx]);
					--modIdx;
				}
			}
		}

		for (int shieldIdx = 0; shieldIdx < shields.Count; ++shieldIdx)
		{
			shields[shieldIdx].SetActive(shieldIdx < shieldCount);
		}

		transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}
}
