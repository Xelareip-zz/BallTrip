using UnityEngine;

public enum BOSS_TYPE
{
	SHIELD,
	HEART,
	ENERGY,
	ROCKET,
}

public class BossBase : MonoBehaviour, ICanSpawn
{
	public BOSS_TYPE bossType;
	
	public GameObject deadVersion;
	public GameObject endLevelDoor;
	

	public virtual bool GetIsAlive()
	{
		return Player.Instance.GetLevelBeaten(bossType) == false;
    }

	public virtual bool CanSpawn()
	{
		return true;
	}
}