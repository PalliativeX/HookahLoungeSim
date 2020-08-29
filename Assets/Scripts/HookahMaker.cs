using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HookahMaker : MonoBehaviour
{
	public NavMeshAgent agent;
	public GameObject selectionCircle;
	public Transform hookahCarryPos;

	Player player;

	bool selected;

	Hookah servedHookah;
	bool carryingHookah;
	Table servedTable;

	public Location CurrentLoc { get; set; }

	Queue<Action> actions;
	Action currentAction;

	void Start()
	{
		CurrentLoc = Location.Lounge;
		actions = new Queue<Action>();
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
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
				Move(moveAction.dest);
			}
			else if (currentAction is TakeHookahAction takeAction)
			{
				HandleTakeHookahAction(takeAction);
			}
			else if (currentAction is BringHookahAction bringAction)
			{
				HandleBringHookahAction(bringAction);
			}
		}

		if (AgentNotMoving() && currentAction != null)
		{
			currentAction = null;
		}
	}

	void Move(Vector3 dest)
	{
		agent.ResetPath();
		agent.destination = dest;
	}

	void HandleTakeHookahAction(TakeHookahAction takeAction)
	{
		if (!carryingHookah)
		{
			Hookah hookah = takeAction.hookah;

			float range = 5f;
			if (Vector3.Distance(transform.position, hookah.transform.position) > range)
			{
				currentAction = null;

				Vector3 closestPoint = Utils.ClosestPointOnSphere(transform.position, hookah.transform.position, range * 0.96f);
				AddMoveAction(closestPoint);
				AddTakeHookahAction(hookah);
			}
			else
			{
				servedHookah = hookah;
				servedHookah.transform.SetParent(transform);
				servedHookah.transform.position = hookahCarryPos.position;
				servedHookah.ContainedTobacco = ChooseTobacco();
				servedHookah.GetComponent<Collider>().enabled = false;
				carryingHookah = true;
				currentAction = null;
			}
		}
	}

	void HandleBringHookahAction(BringHookahAction bringAction)
	{
		Table table = bringAction.table;
		float range = 7f;
		if (Vector3.Distance(transform.position, table.HookahPos) > range)
		{
			currentAction = null;
			Vector3 closestPoint = Utils.ClosestPointOnSphere(transform.position, table.HookahPos, range * 0.9f);
			AddMoveAction(closestPoint);
			AddBringHookahAction(table);
		}
		else
		{
			carryingHookah = false;
			table.Hookah = servedHookah;
			table.Hookah.transform.SetParent(table.transform);
			table.Hookah.transform.position = table.HookahPos;
			servedHookah = null;
			servedTable = table;
		}
	}

	// TODO: This should take into account client prefs
	Tobacco ChooseTobacco()
	{
		return player.GetRandomTobacco();
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
		if (!carryingHookah)
		{
			actions.Enqueue(new TakeHookahAction(hookah));
		}
	}

	public void AddBringHookahAction(Table table)
	{
		if (carryingHookah)
		{
			actions.Enqueue(new BringHookahAction(table));
		}
	}

	public void ClearActions()
	{
		actions.Clear();
		currentAction = null;
	}

	public void UpdateSpeed(float playSpeed)
	{
		agent.speed *= playSpeed;
	}

	bool AgentNotMoving()
	{
		Vector3 pos = agent.transform.position;
		pos.y = 0f;

		if (!agent.pathPending &&
			Vector3.Distance(agent.destination, pos) <= agent.stoppingDistance &&
			!agent.hasPath && agent.velocity.sqrMagnitude == 0f)
		{
			return true;
		}

		return false;
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
