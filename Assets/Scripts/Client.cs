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
public struct ClientPrefs
{
	public FlavourGroup group;
	public Taste taste;
	public Strength strength;

	public ClientPrefs(FlavourGroup group = FlavourGroup.None, Taste taste = Taste.None, Strength strength = Strength.None)
	{
		this.group = group;
		this.taste = taste;
		this.strength = strength;
	}

	public bool SatisfiesPrefs(Tobacco tobacco)
	{
		if (group != FlavourGroup.None && group != tobacco.flavour.group)
		{
			return false;
		}
		if (taste != Taste.None && taste != tobacco.flavour.taste)
		{
			return false;
		}
		if (strength != Strength.None && strength != tobacco.brand.strength)
		{
			return false;
		}

		return true;
	}
}

public class Client : MonoBehaviour
{
	public ChatBubble bubble;
	public NavMeshAgent agent;
	public ClientPrefs prefs;
	public string[] onHookahBrought;

	Player player;

	ClientStatus status;
	HookahMaker worker;
	Table occupiedTable;
	Hookah smokedHookah;
	ClientPrefs currentPrefs;

	float waitingTime;
	bool prefsSatisfied;


    void Start()
    {
		player = FindObjectOfType<Player>();
		currentPrefs = new ClientPrefs();
		Enter();
    }

	void Update()
	{
		if (status == ClientStatus.Entered && NotMoving())
		{
			LookForFreeTable();
		}

		if (status == ClientStatus.Approaching && NotMoving())
		{
			Sit();
		}

		if (status == ClientStatus.Waiting)
		{
			waitingTime += PlayTimer.Instance.TimePerFrame();

			if (occupiedTable.Hookah != null)
			{
				smokedHookah = occupiedTable.Hookah;
				status = ClientStatus.Smoking;
				smokedHookah.SetActive();
				prefsSatisfied = PrefsSatisfied();
				DisplayText(prefsSatisfied ? onHookahBrought[0] : onHookahBrought[1], 3f);
			}
		}

		if (status == ClientStatus.Smoking)
		{
			if (smokedHookah.Active)
			{
				// TODO: Add smoking animation there
			}
			else
			{
				status = ClientStatus.Leaving;
				agent.SetDestination(player.entry.position);
			}
		}

		if (status == ClientStatus.Leaving && NotMoving())
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

	void Enter()
	{
		status = ClientStatus.Entered;
		//Vector3 advance = new Vector3(0.3f, 0, 0);
		//agent.SetDestination(agent.transform.position + advance);
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
			status = ClientStatus.Approaching;
		}
		else
		{
			status = ClientStatus.Leaving;
			agent.SetDestination(player.entry.position);
		}
	}

	void Leave()
	{
		Destroy(transform.gameObject);
	}

	void Sit()
	{
		status = ClientStatus.Waiting;
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

	// NOTE: This can get messy with huge speed, prob increase it by lower rates
	public void UpdateSpeed(float playSpeed)
	{
		agent.speed *= playSpeed;
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

	public ClientStatus Status
	{
		get { return status; }
		set {
			status = value;
		}
	}

}
