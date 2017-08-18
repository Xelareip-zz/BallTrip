using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossShield : BossBase
{
	public InfiniteLevel level;

	public GameObject endLevelDoor;
	public Text hpText;

	public CameraShaker shaker;
	public List<GameObject> shields;

	void Start()
	{
		foreach (InfiniteLevelGoal goal in level.ends)
		{
			goal.goalHit += LevelPassed;
		}
		foreach (CollisionSignal signal in GetComponentsInChildren<CollisionSignal>())
		{
			signal.collisionExit += ShieldDestroyed;
			shields.Add(signal.gameObject);
		}
	}

	public void LevelPassed(InfiniteLevelGoal goal)
	{
		Player.Instance.SetLevelBeaten(bossType);
		Player.Instance.SetShieldLevel(1);
	}

	void ShieldDestroyed(CollisionSignal signal, Collision coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.shieldActive = true;
			shields.Remove(signal.gameObject);
			if (shields.Count == 0)
			{
				Destroy(endLevelDoor);
			}
			Destroy(signal.gameObject);
		}
	}
}
