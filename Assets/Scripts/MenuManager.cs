using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject settingsMenu;

	SettingsManager settingsManager;

	void Start()
	{
		AudioManager.instance.MasterVolumePercent = PlayerPrefs.GetFloat("master");
		AudioManager.instance.MusicVolumePercent  = PlayerPrefs.GetFloat("music");
		AudioManager.instance.SFXVolumePercent    = PlayerPrefs.GetFloat("sfx");

		settingsManager = settingsMenu.GetComponent<SettingsManager>();
	}

	public void Play()
	{
		SceneManager.LoadSceneAsync("MainScene");
	}

	public void SwitchSettingsMenu(bool toggle)
	{
		settingsMenu.SetActive(toggle);
		mainMenu.SetActive(!toggle);
	}

	public void QuitGame()
	{
		settingsManager.SavePlayerPrefs();

		Application.Quit();
	}

}