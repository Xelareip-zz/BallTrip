using System.Collections;
using System.Collections.Generic;
using UnityEngine.Analytics;

public class AnalyticsSender
{
	private static AnalyticsSender instance;
	public static AnalyticsSender Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new AnalyticsSender();
			}
			return instance;
		}
	}

	public AnalyticsResult Send(string eventName, Dictionary<string, object> eventData)
	{
#if UNITY_EDITOR
		return Analytics.CustomEvent(eventName, eventData);
#else
		return AnalyticsResult.Ok;
#endif
	}
}
