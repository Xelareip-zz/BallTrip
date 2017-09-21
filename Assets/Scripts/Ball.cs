using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private static Ball instance;
	public static Ball Instance
	{
		get
		{
			return instance;
		}
	}

	public Rigidbody ballRigidbody;
	public Collider ballCollider;
	public LaunchDirection launchDirectionScript;
	public GameObject shieldVisual;
	public GameObject uiFreeCollisions;
	public GameObject uiHeartLost;

	//public bool shieldActive;
	public int currentHeartCount;
	public int currentHeartCountUI;
	public float endDrag;

	public float launchSpeed;
	public float slowSpeed;

	public Vector3 launchDirection;

	public Vector3 oldVelocity;

	[SerializeField]
	private float hp;

	public int lastLevelHit;
	public int lastLevelHitCount;

	public List<IBallModifier> modifiers;

	void Awake()
	{
		instance = this;
		modifiers = new List<IBallModifier>();
		currentHeartCount = Player.Instance.GetHearts();
		currentHeartCountUI = currentHeartCount;
		//shieldActive = false;
		lastLevelHit = -1;
        hp = Player.Instance.GetEnergy();
	}

	void FixedUpdate()
	{
		UpdateTriggerState();

		Vector3 newVelocity = ballRigidbody.velocity;
		if (hp <= 0)// && shieldActive == false)
		{
			newVelocity = ballRigidbody.velocity.normalized * Mathf.Max(0.0f, ballRigidbody.velocity.magnitude - slowSpeed * Time.fixedDeltaTime);
		}
		else if (ballRigidbody.velocity.magnitude != 0)
		{
			newVelocity = ballRigidbody.velocity * launchSpeed / ballRigidbody.velocity.magnitude;
        }

		for (int modIdx = 0; modIdx < modifiers.Count; ++modIdx)
		{
			if (modifiers[modIdx].VelocityUpdate(ref newVelocity))
			{
				break;
			}
		}
		ballRigidbody.velocity = newVelocity;
	}

	void Update()
	{
		for (int modIdx = 0; modIdx < modifiers.Count; ++modIdx)
		{
			modifiers[modIdx].Update();
		}
		float threshold = 0f;
        if (ballRigidbody.velocity.magnitude <= threshold && oldVelocity.magnitude > threshold)
		{
			hp = Player.Instance.GetEnergy();

			InfiniteLevelsManager.Instance.SetNextHeartLevel();

			if (TutoManager.Instance.StartTuto("TutoSecondSling"))
			{
				InfiniteGameManager.Instance.SetLaunchMode(LAUNCH_MODE.LAUNCH);
			}
			else
			{
				InfiniteGameManager.Instance.SetLaunchMode(LAUNCH_MODE.LOOK);
			}
		}
		oldVelocity = ballRigidbody.velocity;

		//shieldVisual.SetActive(shieldActive);

		Debug.DrawLine(transform.position, transform.position + launchDirection * 10.0f, Color.green, 0.0f);
	}

	public void Launch()
	{
		InfiniteGameManager.Instance.SetLaunchMode(LAUNCH_MODE.FOLLOW);
		ballRigidbody.velocity = launchDirection.normalized * launchSpeed;
		launchDirection = Vector3.zero;
    }

	void OnTriggerEnter(Collider coll)
	{
		for (int modIdx = 0; modIdx < modifiers.Count; ++modIdx)
		{
			if (modifiers[modIdx].OnBallTriggered(coll))
			{
				break;
			}
		}
	}

	void OnCollisionEnter(Collision coll)
	{
		if (InfiniteLevelsManager.Instance.currentLevel != lastLevelHit)
		{
			lastLevelHit = InfiniteLevelsManager.Instance.currentLevel;
			lastLevelHitCount = 0;
        }
		else
		{
			++lastLevelHitCount;
		}
		/*
		Vector3 normal = Vector3.zero;

		foreach (var contact in coll.contacts)
		{
			normal += contact.normal;
		}
		
		normal /= coll.contacts.Length;
		
		
		if (Vector3.Dot(oldVelocity, normal) == 0)
		{
			float dot = Vector3.Dot(normal, oldVelocity);
			Vector3 bouncedPart = normal * dot;
			Vector3 newVelocity = oldVelocity - 2 * bouncedPart;
			ballRigidbody.velocity = newVelocity;
		}
		normal.Normalize();
		*/

		float stressIncrease = (1.0f - Vector3.Dot(coll.contacts[0].normal, oldVelocity)) / launchSpeed;
		//InfiniteGameManager.Instance.cameraShaker.stressLevel += stressIncrease;
    }

	private void LooseHeartUI(int heartLoss, Vector3 direction)
	{
		int lostValue = Mathf.Min(currentHeartCount, heartLoss);
		currentHeartCountUI -= lostValue;
		GameObject heartUi = Instantiate<GameObject>(uiHeartLost);
		heartUi.transform.SetParent(null);
		heartUi.transform.position = uiHeartLost.transform.position;
		heartUi.GetComponentInChildren<MoveInDir>().direction = direction;
		heartUi.SetActive(true);
	}

	void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + launchDirection * launchSpeed);
	}

	public void HeartIncrease(int value = 1, bool animation = false)
	{
		currentHeartCount += value;
		if (animation)
		{
			for (int idx = 0; idx < value; ++idx)
			{
				GameObject newUI = Instantiate<GameObject>(uiFreeCollisions);
				newUI.transform.SetParent(Camera.main.transform);
				newUI.transform.position = Ball.Instance.transform.position;
				newUI.GetComponent<GoTo>().finished += HeartIncreaseApply;
				newUI.SetActive(true);
			}
		}
		else
		{
			currentHeartCountUI += value;
		}
	}

	public void Hit(IObstacle obstacle)
	{
		if (InfiniteGameManager.Instance.GetMode() != LAUNCH_MODE.FOLLOW)
		{
			return;
		}
		TutoManager.Instance.StartTuto("TutoFirstHit");
		bool hadHp = hp > 0;

		float hpToAdd = -Mathf.Max(obstacle.HpLossOnTick(), 0);

		for (int modIdx = 0; modIdx < modifiers.Count; ++modIdx)
		{
			if (modifiers[modIdx].OnDamageReceived(ref hpToAdd))
			{
				break;
			}
		}

		AddHP(hpToAdd);
		//shieldActive = false;

		if (hadHp && hp <= 0)
		{
			--currentHeartCount;
			--currentHeartCountUI;
		}
	}

	public void AddHP(float value)
	{
		hp = Mathf.Clamp(hp + value, 0, Player.Instance.GetEnergy());
		if (value < 0)
		{
			RedCurtainManager.Instance.stressLevel += 1.0f;
		}
	}

	public float GetHp()
	{
		return hp;
	}

	private void HeartIncreaseApply()
	{
		++currentHeartCountUI;
	}

	public void AddModifier(IBallModifier modifier)
	{
		if (modifiers.Contains(modifier) == false)
		{
			modifiers.Add(modifier);
		}
		UpdateTriggerState();
	}

	public void RemoveModifier(IBallModifier modifier)
	{
		while (modifiers.Contains(modifier))
		{
			modifiers.Remove(modifier);
		}
		UpdateTriggerState();
    }

	private void UpdateTriggerState()
	{
		bool found = false;
		for (int modIdx = 0; modIdx < modifiers.Count; ++modIdx)
		{
			if (modifiers[modIdx].BallTriggerMode())
			{
				found = true;
				break;
			}
		}

		ballCollider.isTrigger = found;
	}
}
