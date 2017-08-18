using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.UI;

public class InfiniteLevel : MonoBehaviour
{
	public static int count = 0;

#if UNITY_EDITOR
	public bool baseLevel;
#endif

	public Bounds levelBounds;

	public float closeDelay;
	
	public InfiniteLevelStart start;
	public List<InfiniteLevelGoal> ends;
	public InfiniteLevelDoor door;
	public int reward;

	public List<Vector3> pickupSpots;
	public List<Text> levelTexts;

	public int levelNumber;
	public float spawnChances = 0;
	

	void Awake()
	{
		foreach (InfiniteLevelGoal endLevel in ends)
		{
			endLevel.level = this;
			endLevel.goalHit += GoalPassed;
		}
		start.level = this;
		InfiniteLevelsManager.Instance.RegisterLevel(this);
		foreach (Vector3 pickupSpot in pickupSpots)
		{
			GameObject newPickup = PickupManager.Instance.GetRandomPickup();
			if (newPickup != null)
			{
				GameObject newInstance = Instantiate<GameObject>(newPickup);
				newInstance.transform.SetParent(transform);
				newInstance.transform.localPosition = pickupSpot;
			}
        }
	}

	void Start()
	{
		for (int textIdx = 0; textIdx < levelTexts.Count; ++textIdx)
		{
			levelTexts[textIdx].text = levelNumber.ToString();
		}
	}

	public bool CanSpawn()
	{
		foreach (ICanSpawn canSpawner in GetComponentsInChildren<ICanSpawn>())
		{
			if (canSpawner.CanSpawn() == false)
			{
				return false;
			}
		}
		return true;
	}

	public void CalculateBounds()
	{
		List<Transform> children = new List<Transform>();
		List<Transform> newChildren = new List<Transform>();

		children.Add(transform);
		newChildren.Add(transform);

		while (newChildren.Count != 0)
		{
			List<Transform> nextChildren = new List<Transform>();
			foreach (Transform currentParent in newChildren)
			{
				for (int childIdx = 0; childIdx < currentParent.childCount; ++childIdx)
				{
					Transform child = currentParent.GetChild(childIdx);
					if (child is RectTransform == false)
					{
						nextChildren.Add(child);
					}
				}
			}
			children.AddRange(nextChildren);
			newChildren.Clear();
			newChildren.AddRange(nextChildren);
		}

		levelBounds = XUtils.GetBounds(children);
		levelBounds.center -= transform.position;
	}

	public void GoalPassed(InfiniteLevelGoal goal)
	{
		if (Ball.Instance.shieldActive)
        {
			InfiniteGameManager.Instance.FreeLaunchIncrease();
		}
		else
		{
			TutoManager.Instance.StartTuto("TutoFirstCoin");
		}
		if (Player.Instance.GetShieldLevel() > 0)
		{
			Ball.Instance.shieldActive = true;
			TutoManager.Instance.StartTuto("TutoShieldPortal");
		}
		InfiniteGameManager.Instance.AddScore(reward);
		InfiniteGameManager.Instance.currentColorCursor += 0.1f;
		InfiniteLevelsManager.Instance.RemoveLevel(this, goal.boundStart.level);
	}

	public void CloseLevel()
	{
		StartCoroutine(CloseInDelay());
	}

	public IEnumerator CloseInDelay()
	{
		//yield return new WaitForSeconds(closeDelay);
		door.gameObject.SetActive(true);
		yield return null;
	}

	public int GetDepth()
	{
		if (start.boundEnd == null)
		{
			return 0;
		}

		return start.boundEnd.level.GetDepth() + 1;
	}

	public void RemoveReferences()
	{
		if (start.boundEnd != null)
		{
			start.boundEnd.boundStart = null;
		}
		foreach (InfiniteLevelGoal end in ends)
		{
			if (end.boundStart != null)
			{
				end.boundStart.boundEnd = null;
            }
		}
	}

	public Bounds GetCurrentBounds()
	{
		return new Bounds(levelBounds.center + transform.position, levelBounds.size);
	}
#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (Selection.Contains(gameObject))
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireCube(transform.position + levelBounds.center, levelBounds.size);
		}

		Gizmos.color = Color.blue;
		foreach (Vector3 pickupSpot in pickupSpots)
		{
			Gizmos.DrawSphere(transform.position + pickupSpot, 0.3f);
		}
	}
#endif
}
