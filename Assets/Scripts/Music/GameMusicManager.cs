using UnityEngine;
using System.Collections;

public class GameMusicManager : MonoBehaviour
{
	public bool musicOn;

	public AudioClip[] loungeMusic;
	AudioClip prevPlayed;

	bool currentlyPlaying;

	void Update()
	{
		if (!musicOn || AudioManager.Instance == null || loungeMusic.Length == 0)
			return;

		if (!currentlyPlaying)
		{
			AudioClip clip = ChooseRandomClip();
			StartCoroutine(PlayMusic(clip));
		}
	}

	IEnumerator PlayMusic(AudioClip clip)
	{
		currentlyPlaying = true;
		AudioManager.Instance.PlayMusic(clip);

		yield return new WaitForSeconds(clip.length);

		prevPlayed = clip;
		currentlyPlaying = false;
	}

	AudioClip ChooseRandomClip()
	{
		AudioClip clip = loungeMusic[Random.Range(0, loungeMusic.Length-1)];
		if (clip == prevPlayed)
		{
			clip = loungeMusic[Random.Range(0, loungeMusic.Length - 1)];
		}

		return clip;
	}

}
