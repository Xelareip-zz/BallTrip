using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossHeart : BossBase
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
		Player.Instance.SetLevelBeaten(bossType);
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			shaker.stressLevel += 0.5f;
			StartCoroutine(FreeCollisions(2));
			--hp;
			hp = Mathf.Max(hp, 0);
			if (hp == 0)
			{
				Destroy(endLevelDoor);
				Destroy(gameObject);
			}
		}
	}

	private IEnumerator FreeCollisions(int number)
	{
		for (int idx = 0; idx < number; ++idx)
		{
			Ball.Instance.FreeCollisionsIncrease();
			yield return new WaitForEndOfFrame();
		}
	}
}
