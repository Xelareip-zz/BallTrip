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
	public LineRenderer lineRenderer;
	public GameObject shieldVisual;
	public GameObject uiFreeCollisions;
	
	public bool freeBounce;
	public int currentCollisionCount;
	public float endDrag;

	public float launchSpeed;
	public float slowSpeed;

	public Vector3 launchDirection;

	public Vector3 oldVelocity;

	void Awake()
	{
		instance = this;
		currentCollisionCount = Player.Instance._bounces;
		freeBounce = false;
	}

	void FixedUpdate()
	{
		if (currentCollisionCount <= 0 && freeBounce == false)
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
			freeBounce = true;
			InfiniteGameManager.Instance.SetLaunchMode(LAUNCH_MODE.LOOK);
			currentCollisionCount = Player.Instance._bounces;
		}
		oldVelocity = ballRigidbody.velocity;
		lineRenderer.SetPositions(new Vector3[] { transform.position, transform.position + launchDirection * 10.0f });

		shieldVisual.SetActive(freeBounce);

		Debug.DrawLine(transform.position, transform.position + launchDirection * 10.0f, Color.green, 0.0f);
	}

	public void Launch()
	{
		InfiniteGameManager.Instance.SetLaunchMode(LAUNCH_MODE.FOLLOW);
		ballRigidbody.velocity = launchDirection.normalized * launchSpeed;
		launchDirection = Vector3.zero;
		--InfiniteGameManager.Instance.launchesLeft;
    }

	void OnCollisionEnter(Collision coll)
	{
		Vector3 normal = Vector3.zero;

		foreach (var contact in coll.contacts)
		{
			normal += contact.normal;
		}
		normal.Normalize();

		Vector3 bouncedPart = normal * Vector3.Dot(oldVelocity, normal);
		Vector3 newVelocity = oldVelocity - 2 * bouncedPart;
		ballRigidbody.velocity = newVelocity;

        if (freeBounce)
		{
			freeBounce = false;
		}
		else
		{
			currentCollisionCount = Mathf.Max(0, currentCollisionCount - 1);
		}
		float stressIncrease = (1.0f - Vector3.Dot(coll.contacts[0].normal, oldVelocity)) / launchSpeed;
		InfiniteGameManager.Instance.cameraShaker.stressLevel += stressIncrease;
    }

	void OnDrawGizmos()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(transform.position, transform.position + launchDirection * launchSpeed);
	}

	public void FreeCollisionsIncrease()
	{
		GameObject newUI = Instantiate<GameObject>(uiFreeCollisions);
		newUI.transform.SetParent(Camera.main.transform);
		newUI.transform.position = Ball.Instance.transform.position;
		newUI.GetComponent<GoTo>().finished += FreeCollisionsIncreaseApply;
		newUI.SetActive(true);
	}

	private void FreeCollisionsIncreaseApply()
	{
		++currentCollisionCount;
	}
}
