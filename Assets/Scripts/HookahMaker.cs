using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;


public class HookahMaker : MonoBehaviour
{
	public NavMeshAgent agent;
	public GameObject selectionCircle; 

	bool selected;


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
				Move(moveAction.dest);
			}
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

	public void AddAction(Action action)
	{
		actions.Enqueue(action);
	}

	public void AddMoveAction(Vector3 dest)
	{
		actions.Enqueue(new MoveAction(dest));
	}

	public void ClearActions()
	{
		actions.Clear();
	}

	public bool Selected
	{
		get { return selected; }
		set {
			selected = value;
			selectionCircle.SetActive(value);
		}
	}

}
