using System.Collections.Generic;
using UnityEngine;

public class BossCoinsPU : BossBase
{
	public InfiniteLevel level;
	
	public List<GameObject> pickups;

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
		while(pickups.Contains(null))
		{
			pickups.Remove(null);
		}

		if (pickups.Count == 0 && endLevelDoor != null)
		{
			Destroy(endLevelDoor);
		}
	}
}
