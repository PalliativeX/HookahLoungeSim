using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
	public Slider masterSlider, musicSlider, sfxSlider;
	public Toggle fullscreenToggle;
	public TMPro.TMP_Dropdown resolutionDropdown;
	public TMPro.TMP_Dropdown qualityDropdown;

	Resolution[] resolutions;

	void Start()
    {
		if (AudioManager.instance != null)
		{
			masterSlider.value = AudioManager.instance.MasterVolumePercent;
			musicSlider.value = AudioManager.instance.MusicVolumePercent;
			sfxSlider.value = AudioManager.instance.SFXVolumePercent;
		}

		resolutions = Screen.resolutions;

		resolutionDropdown.ClearOptions();

		List<string> options = new List<string>();

		int currentResolutionIndex = 0;
		for (int i = 0; i < resolutions.Length; i++)
		{
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width &&
				resolutions[i].height == Screen.currentResolution.height)
			{
				currentResolutionIndex = i;
			}
		}

		resolutionDropdown.AddOptions(options);

		// Loading player settings
		if (PlayerPrefs.HasKey("quality"))
		{
			qualityDropdown.value = PlayerPrefs.GetInt("quality");
		}
		else
		{
			qualityDropdown.value = qualityDropdown.options.Count - 1;
		}
		if (PlayerPrefs.HasKey("fullscreen"))
		{
			fullscreenToggle.isOn = PlayerPrefs.GetInt("fullscreen") == 1 ? true : false;
		}
		else
		{
			fullscreenToggle.isOn = true;
			ToggleFullscreen(true);
		}
		if (PlayerPrefs.HasKey("resolution"))
		{
			currentResolutionIndex = PlayerPrefs.GetInt("resolution");
		}
		resolutionDropdown.value = currentResolutionIndex;
		SetResolution(currentResolutionIndex);
		resolutionDropdown.RefreshShownValue();
	}

	public void SetQuality(int qualityIndex)
	{
		QualitySettings.SetQualityLevel(qualityIndex);
	}

	public void ToggleFullscreen(bool isFullscreen)
	{
		Screen.fullScreen = isFullscreen;
	}

	public void SetResolution(int resolutionIndex)
	{
		Resolution resolution = resolutions[resolutionIndex];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void SetMasterVolume(float newVolume)
	{
		AudioManager.instance.MasterVolumePercent = newVolume;
	}

	public void SetMusicVolume(float newVolume)
	{
		AudioManager.instance.MusicVolumePercent = newVolume;
	}

	public void SetSFXVolume(float newVolume)
	{
		AudioManager.instance.SFXVolumePercent = newVolume;
	}

	public void SavePlayerPrefs()
	{
		PlayerPrefs.SetInt("quality", qualityDropdown.value);
		PlayerPrefs.SetInt("fullscreen", fullscreenToggle.isOn ? 1 : 0);
		PlayerPrefs.SetInt("resolution", resolutionDropdown.value);
		if (AudioManager.instance != null)
		{
			PlayerPrefs.SetFloat("master", AudioManager.instance.masterVolumePercent);
			PlayerPrefs.SetFloat("music", AudioManager.instance.musicVolumePercent);
			PlayerPrefs.SetFloat("sfx", AudioManager.instance.sfxVolumePercent);
		}

		PlayerPrefs.Save();
	}

	private void OnApplicationQuit()
	{
		SavePlayerPrefs();
	}
}
