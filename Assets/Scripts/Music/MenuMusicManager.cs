using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMusicManager : MonoBehaviour
{
	public bool musicOn;

	public AudioClip menuTheme;

	private void Awake()
	{
		if (musicOn && menuTheme != null)
		{
			if (AudioManager.Instance != null)
			{
				AudioManager.Instance.PlayMusic(menuTheme);
			}
		}
	}
}
