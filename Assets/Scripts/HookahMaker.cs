using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

// TODO: Probably incapsulate client preferences into a class

public class HookahMaker : MonoBehaviour
{
	public NavMeshAgent agent;
	public GameObject selectionCircle;
	public Transform hookahCarryPos;

	bool selected;

	Hookah servedHookah;
	bool carryingHookah;
	Table servedTable;

	//bool clientPreferences

	//Animator animator;

	public Location CurrentLoc { get; set; }

	Queue<Action> actions;
	Action currentAction;

	void Start()
	{
		CurrentLoc = Location.Lounge;
		actions = new Queue<Action>();
		//animator = GetComponent<Animator>();
	}

	void Update()
	{
		if (currentAction == null && actions.Count != 0)
		{
			currentAction = actions.Dequeue();
		}

		if (currentAction != null && !currentAction.started)
		{
			currentAction.started = true;

			if (currentAction is MoveAction moveAction)
			{
				//animator.SetFloat("Move", 1f);
				moveAction.dest.y = transform.position.y;
				Move(moveAction.dest);
			}
			else if (currentAction is ServeTableAction serveAction)
			{
				Table table = serveAction.table;
				if (table.Occupied)
				{
					servedTable = table;
					table.ClientSitting.GetPreferences();
				}
			}
			else if (currentAction is TakeHookahAction takeAction)
			{
				Hookah hookah = takeAction.hookah;
				if (Vector3.Distance(transform.position, hookah.transform.position) > 6f)
				{
					currentAction = null;
					AddMoveAction(GetClosePos(transform.position, hookah.transform.position));
					AddTakeHookahAction(hookah);
				}
				else
				{
					servedHookah = hookah;
					servedHookah.transform.SetParent(this.transform);
					//servedHookah.GetComponent<Collider>().enabled = false;
					//servedHookah.GetComponent<Rigidbody>().useGravity = false;
					carryingHookah = true;
					currentAction = null;
					servedHookah.transform.position = hookahCarryPos.position;
				}
			}
			else if (currentAction is BringHookahAction bringAction)
			{
				Debug.Log("Bring hookah action!!");
				Table table = bringAction.table;
				if (Vector3.Distance(transform.position, table.transform.position) > 6f)
				{
					Debug.Log(Vector3.Distance(transform.position, table.transform.position));
					currentAction = null;
					AddMoveAction(GetClosePos(transform.position, table.transform.position));
					AddBringHookahAction(table);
				}
				else
				{
					table.Hookah = servedHookah;
					servedHookah.transform.SetParent(table.transform);
					//servedHookah.transform.position ...
					servedHookah = null;
				}
			}
		}

		if (carryingHookah)
		{
			//servedHookah.transform.position = hookahCarryPos.position;
		}

		if (AgentNotMoving() && currentAction != null)
		{
			currentAction = null;
			//	animator.SetFloat("Move", 0);
		}
	}

	// NOTE: This works correctly
	bool AgentNotMoving()
	{
		if (!agent.pathPending &&
			Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance &&
			!agent.hasPath && agent.velocity.sqrMagnitude == 0f)
		{
			return true;
		}

		return false;
	}

	void Move(Vector3 dest)
	{
		agent.SetDestination(dest);
	}

	Vector3 GetClosePos(Vector3 pos1, Vector3 pos2)
	{
		return Vector3.Lerp(pos1, pos2, 0.95f);
	}

	public void AddAction(Action action)
	{
		actions.Enqueue(action);
	}

	public void AddMoveAction(Vector3 dest)
	{
		actions.Enqueue(new MoveAction(dest));
	}

	public void AddTakeHookahAction(Hookah hookah)
	{
		actions.Enqueue(new TakeHookahAction(hookah));
	}

	public void AddBringHookahAction(Table table)
	{
		//if (table == servedTable)
		{
			actions.Enqueue(new BringHookahAction(table));
		}
	}

	public void AddServeTableAction(Table table)
	{
		actions.Enqueue(new ServeTableAction(table));
	}

	public void ClearActions()
	{
		actions.Clear();
		currentAction = null;
	}

	public bool Selected
	{
		get { return selected; }
		set {
			selected = value;
			selectionCircle.SetActive(value);
		}
	}

	public bool HasServedTable
	{
		get { return servedTable != null; }
	}

}
