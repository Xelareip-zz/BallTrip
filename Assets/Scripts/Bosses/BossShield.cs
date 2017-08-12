using UnityEngine;
using UnityEngine.UI;

public class BossShield : BossBase
{
	public int hp;

	public InfiniteLevel level;

	public GameObject endLevelDoor;
	public Text hpText;

	public CameraShaker shaker;

	void Start()
	{
		foreach (InfiniteLevelGoal goal in level.ends)
		{
			goal.goalHit += LevelPassed;
		}
	}

	void Update()
	{
		hpText.color = InfiniteGameManager.Instance.currentColor;
		hpText.text = hp.ToString();
	}

	public void LevelPassed(InfiniteLevelGoal goal)
	{
		Player.Instance.SetLevelBeaten(bossType.ToString());
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			shaker.stressLevel += 0.5f;
			Ball.Instance.currentCollisionCount += 2;
			--hp;
			hp = Mathf.Max(hp, 0);
			if (hp == 0)
			{
				Destroy(endLevelDoor);
				Destroy(gameObject);
			}
		}
	}
}
