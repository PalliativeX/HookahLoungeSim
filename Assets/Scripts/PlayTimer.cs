using System;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
	public static PlayTimer Instance { get; private set; }

	public float playSpeed;
	public int startingYear, startingMonth, startingDay;
	public int startingHours;

	public bool SpeedChanged { get; set; }

	DateTime playTime; // NOTE: By default 1 realtime second == 1 ingame minute

	void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
		}
	}

	void Start()
    {
		playTime = new DateTime(startingYear, startingMonth, startingDay, 
							    startingHours, 0, 0);
		playSpeed = 1f;
	}

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.PageUp))
		{
			playSpeed *= 2f;
			SpeedChanged = true;
		}
		else if (Input.GetKeyDown(KeyCode.PageDown))
		{
			playSpeed /= 2f;
			SpeedChanged = true;
		}

		// NOTE: We convert 1 realtime second to 1 ingame minute
		playTime = playTime.AddMinutes(Time.deltaTime * playSpeed);
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

	public float TimePerFrame()
	{
		return (Time.deltaTime * playSpeed);
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
