using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameMenuManager : MonoBehaviour
{
	public Button menuButton;
	public GameObject menuUI;
	public GameObject gameSettingsUI;
	public GameObject saveLoadHandler;
	public SaveLoadMenu saveLoadMenu;

	public TransparentObjectsController transparentObjectsController;

	public TMP_Text gameSpeedText;

	public Button pauseGameButton;
	public Button resumeGameButton;

	SettingsManager settingsManager;

	private void Start()
	{
		settingsManager = gameSettingsUI.GetComponent<SettingsManager>();
		ChangeGameSpeedText();

		SwitchGamePause(false);
	}

	public void OpenGameMenu(bool toggle)
	{
		menuButton.gameObject.SetActive(!toggle);
		menuUI.SetActive(toggle);
		PlayTimer.Instance.SwitchGamePaused(paused: toggle);
		ChangeGameSpeedText();
	}

	public void OpenSettingsMenu(bool toggle)
	{
		menuUI.SetActive(!toggle);
		gameSettingsUI.SetActive(toggle);
	}

	public void OpenSaveMenu()
	{
		menuUI.SetActive(true);
		saveLoadHandler.gameObject.SetActive(true);
		saveLoadMenu.Open(saveMode: true);
	}

	public void OpenLoadMenu()
	{
		menuUI.SetActive(true);
		saveLoadHandler.gameObject.SetActive(true);
		saveLoadMenu.Open(saveMode: false);
	}

	public void CloseLoadSaveMenu()
	{
		menuUI.SetActive(true);
		saveLoadHandler.gameObject.SetActive(false);
	}

	public void ToggleLoadSaveMenu(bool toggle)
	{
		menuUI.SetActive(!toggle);
		saveLoadMenu.gameObject.SetActive(toggle);
	}

	public void ExitToMainMenu()
	{
		SceneManager.LoadSceneAsync("MenuScene");
	}

	public void ChangeGameSpeed(bool increase)
	{
		PlayTimer.Instance.ChangeTimeScale(increase);
		ChangeGameSpeedText();
	}

	public void ChangeObjectsTransparency()
	{
		transparentObjectsController.ChangeTransparency();
	}

	void ChangeGameSpeedText()
	{
		float gameSpeed = PlayTimer.Instance.GameSpeed;
		if (gameSpeed == 0)
		{
			gameSpeedText.text = "Paused";
		}
		else
		{
			gameSpeedText.text = PlayTimer.Instance.GameSpeed + "x";
		}
	}

	public void SwitchGamePause(bool pause)
	{
		resumeGameButton.gameObject.SetActive(pause);
		pauseGameButton.gameObject.SetActive(!pause);
		PlayTimer.Instance.SwitchGamePaused(pause);
		ChangeGameSpeedText();
	}

	private void OnApplicationQuit()
	{
		settingsManager.SavePlayerPrefs();
	}
}
