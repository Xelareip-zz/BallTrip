using System.Collections.Generic;
using UnityEngine;

public class BossShield : BossBase
{
	public InfiniteLevel level;

	public CameraShaker shaker;
	public List<GameObject> shields;

	void Start()
	{
		foreach (InfiniteLevelGoal goal in level.ends)
		{
			goal.goalHit += LevelPassed;
		}
	}

	public void LevelPassed(InfiniteLevelGoal goal)
	{
		Player.Instance.SetLevelBeaten(bossType);
	}

	public override bool CanSpawn()
	{
		return GetIsAlive();
	}

	void Update()
	{
		while(shields.Contains(null))
		{
			shields.Remove(null);
		}

		if (shields.Count == 0 && endLevelDoor != null)
		{
			Destroy(endLevelDoor);
		}
	}
}
