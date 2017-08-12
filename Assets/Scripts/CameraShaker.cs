using UnityEngine;

public class CameraShaker : MonoBehaviour
{
	public float stressLevel;
	public float stressMultiplicator;
	public float stressMultiplicatorTranslation;
	public float stressMultiplicatorRotation;

	public float stressRecovery;
	public float minStressRecovery;
    public GameObject meshRenderer;

	void Update()
	{
		stressLevel = stressLevel - Mathf.Max(stressLevel * stressRecovery, minStressRecovery) * Time.deltaTime;
		stressLevel = Mathf.Clamp01(stressLevel);

		float sampler = Time.time * stressLevel * stressLevel * stressMultiplicator;

		float moveX = Mathf.PerlinNoise(sampler, sampler) - 0.5f;
		float moveY = Mathf.PerlinNoise(sampler + 10, sampler + 10) - 0.5f;
		float rotZ = Mathf.PerlinNoise(sampler + 20, sampler + 20) - 0.5f;
		
		meshRenderer.transform.localPosition = new Vector3(moveX * stressLevel * stressMultiplicatorTranslation, moveY * stressLevel * stressMultiplicatorTranslation, meshRenderer.transform.localPosition.z);
		meshRenderer.transform.localRotation = Quaternion.AngleAxis(rotZ * stressLevel * stressMultiplicatorRotation, Vector3.forward);
    }
}
