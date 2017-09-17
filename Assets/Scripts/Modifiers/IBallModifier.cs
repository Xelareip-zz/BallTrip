using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBallModifier
{
	bool Update();
	bool VelocityUpdate(ref Vector3 velocity);
	bool OnDamageReceived(ref float value);
	bool OnBallTriggered(Collider coll);
	bool BallTriggerMode();
}
