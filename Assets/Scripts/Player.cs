using UnityEngine;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public float startingMoney = 1000f;
	public WorkingHours workingHours;
	public PlayTimer playTimer;
	public Table[] tables;
	public Hookah[] hookahs;
	public List<Client> clients;

	public Transform entry;

	public Transform chatBubblePrefab;

	PlayerGUI playerGUI;
	float money;
	float rating;
	int commentCount;
	WorkStatus workStatus;

	void Awake()
	{
		playerGUI = GetComponent<PlayerGUI>();
		workStatus = WorkStatus.Open;
		money = startingMoney;
		rating = 0;
		ChatBubble.prefab = chatBubblePrefab;
	}

	private void Update()
	{
		WorkStatus = playTimer.GetStatus(workingHours);
	}

	public bool HasFreeTables()
	{
		foreach (Table table in tables)
		{
			if (!table.Occupied)
				return true;
		}
		return false;
	}

	public Table GetFreeTable()
	{
		foreach (Table table in tables)
		{
			if (!table.Occupied)
				return table;
		}
		return null;
	}

	public bool HasFreeHookahs()
	{
		foreach (Hookah hookah in hookahs)
		{
			if (!hookah.Occupied)
				return true;
		}
		return false;
	}

	public Hookah GetFreeHookah()
	{
		foreach (Hookah hookah in hookahs)
		{
			if (!hookah.Occupied)
				return hookah;
		}
		return null;
	}

	public float GetPlaySpeed()
	{
		return playTimer.playSpeed;
	}

	public void AddClient(Client newClient)
	{
		clients.Add(newClient);
	}

	public float Money
	{
		get { return money; }
		set {
			money = value;
			playerGUI.UpdateMoneyText();
		}
	}

	public WorkStatus WorkStatus
	{
		get { return workStatus; }
		set {
			workStatus = value;
			playerGUI.UpdateStatusSignText();
		}
	}

	public float Rating
	{
		get { return rating; }
		set {
			commentCount++;
			rating = (rating + value) / commentCount;
			playerGUI.UpdateRatingText();
		}
	}

}

[System.Serializable]
public struct WorkingHours
{
	public int beginning;
	public int ending;

	public WorkingHours(int beginning, int ending)
	{
		this.beginning = beginning;
		this.ending = ending;
	}
}

public enum WorkStatus
{
	Open, Closed
}
