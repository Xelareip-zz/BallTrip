using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoSpawnBlock : MonoBehaviour, ICanSpawn
{
	public string tuto;

	public bool CanSpawn()
	{
		return Player.Instance.GetTutoFinished(tuto) == false;
	}
}
