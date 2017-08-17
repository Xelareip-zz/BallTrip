using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupBase : MonoBehaviour
{
	public bool active;

	public abstract float GetDropWeight();
}
