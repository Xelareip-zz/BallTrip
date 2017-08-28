﻿using UnityEngine;

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
	public LaunchDirection launchDirectionScript;
	public GameObject shieldVisual;
	public GameObject uiFreeCollisions;
	public GameObject uiHeartLost;

	public bool shieldActive;
	public int currentHeartCount;
	public int currentHeartCountUI;
	public float endDrag;

	public float launchSpeed;
	public float slowSpeed;

	public Vector3 launchDirection;

	public Vector3 oldVelocity;

	public float hp;

	public int lastLevelHit;
	public int lastLevelHitCount;

	void Awake()
	{
		instance = this;
		currentHeartCount = Player.Instance.GetHearts();
		currentHeartCountUI = currentHeartCount;
		shieldActive = false;
		lastLevelHit = -1;
        hp = Player.Instance.GetEnergy();
	}

	void FixedUpdate()
	{
		if (hp <= 0 && shieldActive == false)
		{
			ballRigidbody.velocity = ballRigidbody.velocity.normalized * Mathf.Max(0.0f, ballRigidbody.velocity.magnitude - slowSpeed * Time.fixedDeltaTime);
		}
		else if (ballRigidbody.velocity.magnitude != 0)
		{
			ballRigidbody.velocity *= launchSpeed / ballRigidbody.velocity.magnitude;
        }
	}

	void Update()
	{
		if (ballRigidbody.velocity.magnitude == 0 && oldVelocity.magnitude != 0)
		{
			if (Player.Instance.GetShieldLevel() > 0)
			{
				shieldActive = true;
			}
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

		shieldVisual.SetActive(shieldActive);

		Debug.DrawLine(transform.position, transform.position + launchDirection * 10.0f, Color.green, 0.0f);
	}

	public void Launch()
	{
		InfiniteGameManager.Instance.SetLaunchMode(LAUNCH_MODE.FOLLOW);
		ballRigidbody.velocity = launchDirection.normalized * launchSpeed;
		launchDirection = Vector3.zero;
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

		Vector3 normal = Vector3.zero;

		foreach (var contact in coll.contacts)
		{
			normal += contact.normal;
		}
		normal.Normalize();

		Vector3 bouncedPart = normal * Vector3.Dot(oldVelocity, normal);
		Vector3 newVelocity = oldVelocity - 2 * bouncedPart;
		ballRigidbody.velocity = newVelocity;
		
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

	void OnDrawGizmos()
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
		hp -= obstacle.HpLossOnTick();
		shieldActive = false;

		if (hadHp && hp <= 0)
		{
			--currentHeartCount;
			--currentHeartCountUI;
		}
	}

	private void HeartIncreaseApply()
	{
		++currentHeartCountUI;
	}
}
