using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	public GameObject mainMenu;
	public GameObject settingsMenu;
	public SaveLoadMenu loadGameMenu;
	public GameObject loadingScreen;

	public Image loadingFillImage;

	SettingsManager settingsManager;

	void Start()
	{
		AudioManager.Instance.MasterVolumePercent = PlayerPrefs.GetFloat("master");
		AudioManager.Instance.MusicVolumePercent  = PlayerPrefs.GetFloat("music");
		AudioManager.Instance.SFXVolumePercent    = PlayerPrefs.GetFloat("sfx");

		settingsManager = settingsMenu.GetComponent<SettingsManager>();

		loadingFillImage.fillAmount = 0f;

		PlayerPrefs.DeleteKey("load path");
	}

	public void NewGame()
	{
		mainMenu.SetActive(false);
		mainMenu.SetActive(false);
		loadGameMenu.gameObject.SetActive(false);
		loadingScreen.SetActive(true);

		StartCoroutine(LoadAsynchronously());
	}

	IEnumerator LoadAsynchronously()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");

		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress / .9f);
			loadingFillImage.fillAmount = progress;

			yield return null;
		}
	}

	public void SwitchSettingsMenu(bool toggle)
	{
		settingsMenu.SetActive(toggle);
		mainMenu.SetActive(!toggle);
	}

	public void SwitchLoadGameMenu(bool open)
	{
		loadGameMenu.gameObject.SetActive(open);
		if (open)
		{
			loadGameMenu.Open(false);
		}
		mainMenu.SetActive(!open);
	}

	public void QuitGame()
	{
		settingsManager.SavePlayerPrefs();

		Application.Quit();
	}

}