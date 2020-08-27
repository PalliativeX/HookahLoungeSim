using System;
using UnityEngine;

/* 
 * For now 1 realtime second == 1 ingame minute ~
 * However, we can have different FPS, so fix this approach,
 * Maybe restrict the FPS to 60
 */
public class PlayTimer : MonoBehaviour
{
	public int startingYear, startingMonth, startingDay;
	public int startingHours;
	public float advance = 0.2f; // NOTE: Advance in game seconds per frame

	public float playSpeed;

	DateTime playTime;

    void Start()
    {
		playTime = new DateTime(startingYear, startingMonth, startingDay, 
							    startingHours, 0, 0);
		playSpeed = 1f;
	}

    void Update()
    {
		playTime = playTime.AddSeconds(advance);

		if (Input.GetKeyDown(KeyCode.PageUp))
		{
			advance *= 2f;
			playSpeed *= 2f;
		}
		else if (Input.GetKeyDown(KeyCode.PageDown))
		{
			advance /= 2f;
			playSpeed /= 2f;
		}
    }

	public DateTime GetTime()
	{
		return playTime;
	}

	// NOTE: The format of output is as follows: 06:16 Monday 2015
	public string GetTimeStr()
	{
		return playTime.ToString("hh:mm dddd yyyy", System.Globalization.CultureInfo.InvariantCulture);
	}

	public WorkStatus GetStatus(WorkingHours workingHours)
	{
		if (playTime.Hour >= workingHours.beginning && playTime.Hour < workingHours.ending)
		{
			return WorkStatus.Open;
		}
		else
		{
			return WorkStatus.Closed;
		}
	}
}
