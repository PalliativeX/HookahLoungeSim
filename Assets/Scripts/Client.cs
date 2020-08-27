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

public class Client : MonoBehaviour
{
	public ChatBubble bubble;
	public NavMeshAgent agent;
	public Flavour[] preferredFlavours;
	public FlavourGroup preferredFlavourGroup;
	public Strength  preferredStrength;

	Player player;

	ClientStatus status;
	HookahMaker worker;
	Table occupiedTable;
	Hookah smokedHookah;


	float waitingTime;

    void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
			waitingTime += Time.deltaTime;
		}

		if (status == ClientStatus.Leaving && NotMoving())
		{
			Leave();
		}

		//if (!NotMoving())
		{
			//Debug.Log(transform.rotation.eulerAngles);
			bubble.transform.localRotation = Quaternion.Euler(0, 50 + -transform.localRotation.eulerAngles.y, 0);
		}
	}

	// TODO: Make this non-void
	public void GetPreferences()
	{

	}

	void Enter()
	{
		status = ClientStatus.Entered;
		Vector3 advance = new Vector3(0.3f, 0, 0);
		agent.SetDestination(agent.transform.position + advance);
	}

	void LookForFreeTable()
	{
		if (player.HasFreeTables())
		{
			occupiedTable = player.GetFreeTable();
			occupiedTable.Occupied = true;
			occupiedTable.ClientSitting = this;
			Vector3 destToTable = Vector3.Lerp(agent.transform.position, occupiedTable.sofaPos.position, 0.95f);

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
		//ChatBubble.Create(transform, new Vector3(1.5f, 1.5f), "I want something fresh!");
	}

	void CommentOnLounge()
	{
		// TODO: Add actual calculation later
		player.Rating += 5f;
	}

	bool NotMoving()
	{
		if (!agent.pathPending &&
			Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance &&
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
