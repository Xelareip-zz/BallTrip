using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, IObstacle
{
	#region GUID_IDENTIFIED
	[SerializeField]
	private string GUID = null;

	public void SetGUID()
	{
		if (GetGUID() == null)
		{
			GUID = Guid.NewGuid().ToString();
		}
	}

	public string GetGUID()
	{
		return GUID;
	}
	#endregion

	public int hpCost;
	public Vector4 offset;
	public bool destroyOnHit;
	public bool isTrigger;
	public MeshRenderer meshRenderer;

	void Awake()
	{
		foreach (CollisionSignal signal in GetComponentsInChildren<CollisionSignal>())
		{
			signal.collisionEnter += Signal_collisionEnter;
		}
		foreach (TriggerSignal signal in GetComponentsInChildren<TriggerSignal>())
		{
			signal.collisionEnter += Signal_collisionEnter1;
		}
		gameObject.SetActive(false);
	}

	void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
		Collider coll = GetComponent<Collider>();
		if (coll != null)
		{
			isTrigger = coll.isTrigger;
		}
		if (meshRenderer != null)
		{
			meshRenderer.material = Resources.Load<Material>(isTrigger ? "ObstacleTransMaterial" : "ObstacleMaterial");
		}

	}

	private void Signal_collisionEnter1(Collider coll)
	{
		OnTriggerEnter(coll);
	}

	private void Signal_collisionEnter(CollisionSignal signal, Collision coll)
	{
		OnCollisionEnter(coll);
	}

	void OnCollisionEnter(Collision coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.Hit(this);
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject == Ball.Instance.gameObject)
		{
			Ball.Instance.Hit(this);
			if (destroyOnHit)
			{
				Destroy(gameObject);
			}
		}
	}

	public float HpLossOnTick()
	{
		return hpCost;
	}

	void Update()
	{
		//offset = new Vector4(0.5f - (transform.position.x / transform.localScale.x) % 1, 0.5f - (transform.position.y / transform.localScale.y) % 1, 0.0f, 0.0f);
		//meshRenderer.material.SetFloat("repeatsX", Mathf.RoundToInt(transform.localScale.x));
		//meshRenderer.material.SetFloat("repeatsY", Mathf.RoundToInt(transform.localScale.y));
	}
}
