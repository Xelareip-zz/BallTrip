using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchFirstScene : MonoBehaviour
{
	void Start ()
	{
		if (Player.Instance.GetTutoFinished("TutoFirstLaunch"))
		{
			SceneManager.LoadScene("InfiniteLevelsMenu");
		}
		else
		{
			SceneManager.LoadScene("InfiniteLevels");
		}
	}
}
