using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

	public AnalyticsSender()
	{
		Amplitude amplitude = Amplitude.Instance;
		amplitude.logging = true;
		amplitude.init("09f44615f8f8130ff0a43ce637cfc676");
	}

	public AnalyticsResult Send(string eventName, Dictionary<string, object> eventData)
	{
#if UNITY_EDITOR
		return AnalyticsResult.Ok;
#else
		Amplitude.Instance.logEvent(eventName, eventData);
		return Analytics.CustomEvent(eventName, eventData);
#endif
	}
}
