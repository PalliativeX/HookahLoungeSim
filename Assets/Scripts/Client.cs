﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;

public enum ClientStatus
{
	Entered,
	Approaching,
	Waiting,
	Smoking,
	Leaving
}

[System.Serializable]
public class Client : MonoBehaviour
{
	public string prefabName;

	public ChatBubble bubble;
	public NavMeshAgent agent;
	public ClientPrefs prefs;
	public string[] onHookahBrought;

	public ClientStatus Status { get; set; }

	[HideInInspector]
	public ClientData clientData;

	Player player;
	public Table occupiedTable;
	public Hookah smokedHookah;
	ClientPrefs currentPrefs;

	float waitingTime;
	bool prefsSatisfied;

    void Start()
    {
		player = FindObjectOfType<Player>();
		currentPrefs = new ClientPrefs();
    }

	void Update()
	{
		if (Status == ClientStatus.Entered && NotMoving())
		{
			LookForFreeTable();
		}

		if (Status == ClientStatus.Approaching && NotMoving())
		{
			Sit();
		}

		if (Status == ClientStatus.Waiting)
		{
			waitingTime += PlayTimer.Instance.TimePerFrame();

			if (occupiedTable.Hookah != null)
			{
				smokedHookah = occupiedTable.Hookah;
				Status = ClientStatus.Smoking;
				smokedHookah.SetActive();
				prefsSatisfied = PrefsSatisfied();
				DisplayText(prefsSatisfied ? onHookahBrought[0] : onHookahBrought[1], 3f);
			}
		}

		if (Status == ClientStatus.Smoking)
		{
			if (smokedHookah.Active)
			{
				// TODO: Add smoking animation there
			}
			else
			{
				Status = ClientStatus.Leaving;
				agent.SetDestination(player.entry.position);
			}
		}

		if (Status == ClientStatus.Leaving && NotMoving())
		{
			player.RemoveClient(this);
			if (waitingTime > 0f)
			{
				CommentOnLounge();
				player.Money += smokedHookah.ContainedTobacco.brand.price;
			}
			Leave();
		}

		// NOTE: Updating chat bubble so that it does not rotate with a parent object
		bubble.transform.localRotation = Quaternion.Euler(0, bubble.initialRotation + -transform.localRotation.eulerAngles.y, 0);
	}

	public string GetPrefs()
	{
		string prefsStr = "I want ";
		currentPrefs = new ClientPrefs();

		if (prefs.strength != Strength.None)
		{
			switch (prefs.strength)
			{
				case Strength.Soft:
					prefsStr += "a soft hookah";
					currentPrefs.strength = Strength.Soft;
					break;
				case Strength.Medium:
					prefsStr += "a medium strength hookah";
					currentPrefs.strength = Strength.Medium;
					break;
				case Strength.Strong:
					prefsStr += "a strong hookah";
					currentPrefs.strength = Strength.Strong;
					break;
			}
		}
		else if (prefs.group != FlavourGroup.None)
		{
			string groupStr = prefs.group.ToString();
			prefsStr += "a " + groupStr.ToLower() + " hookah";
			currentPrefs.group = prefs.group;
		}	
		else if (prefs.taste != Taste.None)
		{
			string tasteStr = prefs.taste.ToString();
			prefsStr += "a " + tasteStr.ToLower() + " taste";
			currentPrefs.taste = prefs.taste;
		}

		return prefsStr;
	}

	bool PrefsSatisfied()
	{
		return prefs.SatisfiesPrefs(smokedHookah.ContainedTobacco);
	}

	public void Enter()
	{
		Status = ClientStatus.Entered;
	}

	void LookForFreeTable()
	{
		if (player.HasFreeTables())
		{
			occupiedTable = player.GetFreeTable();
			occupiedTable.Occupied = true;
			occupiedTable.ClientSitting = this;

			Vector3 destToTable = Utils.ClosestPointOnSphere(transform.position, occupiedTable.approachPlace.position, 3 * 0.96f);
			agent.SetDestination(destToTable);
			Status = ClientStatus.Approaching;
		}
		else
		{
			Status = ClientStatus.Leaving;
			agent.SetDestination(player.entry.position);
		}
	}

	void Leave()
	{
		Destroy(transform.gameObject);
	}

	void Sit()
	{
		Status = ClientStatus.Waiting;
		DisplayText(GetPrefs(), 4f);
	}

	void DisplayText(string text, float displayLength)
	{
		bubble.gameObject.SetActive(true);

		bubble.Display(displayLength, text);
	}

	void CommentOnLounge()
	{
		float Rating = 5f;

		if (waitingTime >= 30f)
		{
			Rating -= 1f;
		}
		if (!prefsSatisfied)
		{
			Rating -= 1f;
		}

		player.Rating += Rating;
	}

	bool NotMoving()
	{
		Vector3 agentPos = agent.transform.position;
		agentPos.y = 0f;

		if (!agent.pathPending &&
			Vector3.Distance(agent.destination, agentPos) <= agent.stoppingDistance &&
			!agent.hasPath && agent.velocity.sqrMagnitude == 0f)
		{
			return true;
		}

		return false;
	}

	// TODO: We must save occupiedTable and smokedHookah
	public void Save(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, currentPrefs);
		formatter.Serialize(stream, waitingTime);
		formatter.Serialize(stream, prefsSatisfied);
		formatter.Serialize(stream, (int)Status);

		formatter.Serialize(stream, new SerializedTransform(transform));
	}

	public void Load(BinaryFormatter formatter, FileStream stream)
	{
		currentPrefs = (ClientPrefs)formatter.Deserialize(stream);
		waitingTime = (float)formatter.Deserialize(stream);
		prefsSatisfied = (bool)formatter.Deserialize(stream);
		Status = (ClientStatus)formatter.Deserialize(stream);

		SerializedTransform serializedTransform = (SerializedTransform)formatter.Deserialize(stream);
		TransformDeserializer.Deserialize(serializedTransform, transform);
	}

}
