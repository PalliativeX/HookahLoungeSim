using System.Collections;
using System;
using System.IO;
using UnityEngine;

public class Door : MonoBehaviour
{
	public Animator leftDoorAnimator;
	public Animator rightDoorAnimator;

	public bool CurrentlyOpen { get; set; }

	private Player player;

	WorkingHours workingHours;

	private void Start()
	{
		player = FindObjectOfType<Player>();
		workingHours = player.workingHours;
		CurrentlyOpen = false;
	}

	void Update()
	{
		DateTime time = PlayTimer.Instance.GetTime();
		float hours = time.Hour;
		// NOTE: If open
		if (hours >= workingHours.beginning && hours <= workingHours.ending)
		{
			if (!CurrentlyOpen) Open();
		}
		else
		{
			if (CurrentlyOpen) Close();
		}
	}

	public void Open()
	{
		ChangeState(open: true);
	}

	public void Close()
	{
		ChangeState(open: false);
	}

	private void ChangeState(bool open)
	{
		CurrentlyOpen = open;
		leftDoorAnimator.SetBool("Open", open);
		rightDoorAnimator.SetBool("Open", open);
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write(CurrentlyOpen);
	}

	public void Load(BinaryReader reader)
	{
		CurrentlyOpen = reader.ReadBoolean();
		ChangeState(open: CurrentlyOpen);
	}
}
