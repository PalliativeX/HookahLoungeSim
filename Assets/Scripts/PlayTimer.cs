using System;
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayTimer : MonoBehaviour
{
	public static PlayTimer Instance { get; private set; }

	public int startingYear, startingMonth, startingDay;
	public int startingHours;

	public float maxTimeScale = 8f;
	public float minTimeScale = 0.125f;

	float timeScale;
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

		timeScale = Time.timeScale;
	}

	void Update()
    {
		// NOTE: We convert 1 realtime second to 1 ingame minute
		playTime = playTime.AddMinutes(Time.deltaTime);
	}

	public DateTime GetTime()
	{
		return playTime;
	}

	public void SwitchGamePaused(bool paused)
	{
		Time.timeScale = paused ? 0 : timeScale;
	}

	public void ChangeTimeScale(bool increase)
	{
		Time.timeScale = increase ? Time.timeScale * 2 : Time.timeScale / 2;
		Time.timeScale = Mathf.Clamp(Time.timeScale, minTimeScale, maxTimeScale);
		timeScale = Time.timeScale;
	}

	// NOTE: The format of output is as follows: 06:16 Monday 2015
	public string GetTimeStr()
	{
		return playTime.ToString("hh:mm dddd yyyy", System.Globalization.CultureInfo.InvariantCulture);
	}

	public float TimePerFrame()
	{
		return (Time.deltaTime);
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

	public void Save(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, timeScale);
		formatter.Serialize(stream, playTime);
	}

	public void Load(BinaryFormatter formatter, FileStream stream)
	{
		timeScale = (float)formatter.Deserialize(stream);
		playTime = (DateTime)formatter.Deserialize(stream);

		//timeScale = reader.ReadSingle();
		//playTime = new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
	}

	public float GameSpeed
	{
		get { return Time.timeScale; }
	}

	public bool IsPaused
	{
		get { return Time.timeScale == 0; }
	}
}
