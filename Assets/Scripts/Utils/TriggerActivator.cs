using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITriggerActivatorReceiver
{
	void Activated();
}

public class TriggerActivator : MonoBehaviour
{
	public List<GameObject> receivers;
	public List<GameObject> targets;
	public bool destroyOnHit;
	public bool activateOnEnter;
	public bool activateOnExit;

	private void Activate()
	{
		for (int targetIdx = 0; targetIdx < targets.Count; ++targetIdx)
		{
			targets[targetIdx].SetActive(true);
		}
		for (int receiveridx = 0; receiveridx < receivers.Count; ++receiveridx)
		{
			if (receivers[receiveridx] == null)
			{
				continue;
			}
			ITriggerActivatorReceiver[] iReceivers = receivers[receiveridx].GetComponentsInChildren<ITriggerActivatorReceiver>();
			for (int iReceiverIdx = 0; iReceiverIdx < iReceivers.Length; ++iReceiverIdx)
			{
				iReceivers[iReceiverIdx].Activated();
			}
		}
		if (destroyOnHit)
		{
			Destroy(gameObject);
		}
	}

	void OnTriggerEnter(Collider coll)
	{
		if (activateOnEnter && coll.gameObject == Ball.Instance.gameObject)
		{
			Activate();
		}
	}

	void OnTriggerExit(Collider coll)
	{
		if (activateOnExit && coll.gameObject == Ball.Instance.gameObject)
		{
			Activate();
		}
	}
}
