using UnityEngine;

public enum BOSS_TYPE
{
	SHIELD,
}

public class BossBase : MonoBehaviour, ICanSpawn
{
	public BOSS_TYPE bossType;

	public bool CanSpawn()
	{
		return Player.Instance.GetLevelBeaten(bossType.ToString()) == false;
	}
}