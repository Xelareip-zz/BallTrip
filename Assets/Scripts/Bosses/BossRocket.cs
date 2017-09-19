using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossRocket: BossBase
{
	public InfiniteLevel level;

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
}
