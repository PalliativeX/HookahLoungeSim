using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.N))
		{
			Play();
		}
	}

	public void Play()
	{
		SceneManager.LoadSceneAsync("MainScene");
	}
}
