using UnityEngine;
using UnityEngine.UI;

public class RedCurtainManager : MonoBehaviour
{
	private static RedCurtainManager instance;
	public static RedCurtainManager Instance
	{
		get
		{
			return instance;
		}
	}
	public float stressLevel;
	public float stressMultiplicator;
	public float stressMultiplicatorTranslation;
	public float stressMultiplicatorRotation;

	public float stressRecovery;
	public float minStressRecovery;

	public Image redCurtain;
	public float maxAlpha;

	void Awake()
	{
		instance = this;
	}

	void Update()
	{
		stressLevel = stressLevel - Mathf.Max(stressLevel * stressRecovery, minStressRecovery) * Time.deltaTime;
		stressLevel = Mathf.Clamp01(stressLevel);

		redCurtain.enabled = stressLevel != 0;

		Color newColor = redCurtain.color;
		newColor.a = maxAlpha * stressLevel * stressLevel;
		redCurtain.color = newColor;
	}
}
