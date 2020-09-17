using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class HookahMaker : MonoBehaviour
{
	public string prefabName;

	public new string name;
	public int cost;
	public Sprite icon;

	public NavMeshAgent agent;
	public GameObject selectionCircle;
	public Transform hookahCarryPos;
	public TobaccoSelectionPanel tobaccoPanel;

	KeyCode CloseChoosingTobaccoPanel = KeyCode.Escape;

	bool selected;

	[HideInInspector]
	public Hookah servedHookah;
	bool choosingTobacco;
	bool carryingHookah;
	[HideInInspector]
	public Table servedTable;

	[HideInInspector]
	public HookahMakerData hookahMakerData;

	Queue<Action> actions;
	Action currentAction;

	void Awake()
	{
		actions = new Queue<Action>();
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
			else if (currentAction is ChooseTobaccoAction chooseAction)
			{
				HandleChooseTobaccoAction(chooseAction);
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

		if (Input.GetKeyDown(CloseChoosingTobaccoPanel) && choosingTobacco)
		{
			CancelChoosingTobacco();
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

	void HandleChooseTobaccoAction(ChooseTobaccoAction chooseAction)
	{
		if (!carryingHookah)
		{
			Hookah hookah = chooseAction.hookah;

			float range = 5f;
			if (Vector3.Distance(transform.position, hookah.transform.position) > range)
			{
				currentAction = null;

				Vector3 closestPoint = Utils.ClosestPointOnSphere(transform.position, hookah.transform.position, range * 0.96f);
				AddMoveAction(closestPoint);
				AddChooseTobaccoAction(hookah);
			}
			else
			{
				choosingTobacco = true;
				servedHookah = hookah;
				DisplayTobaccoPanel(true);
			}
		}
	}

	void HandleTakeHookahAction(TakeHookahAction takeAction)
	{
		servedHookah.GetComponent<Collider>().enabled = false;
		servedHookah.transform.SetParent(transform);
		servedHookah.transform.position = hookahCarryPos.position;
		servedHookah.ContainedTobacco = takeAction.tobacco;
		carryingHookah = true;
		choosingTobacco = false;
		currentAction = null;
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

	public void ChooseTobacco()
	{
		currentAction = null;
		AddTakeHookahAction(tobaccoPanel.FinallyChosenTobacco);
		DisplayTobaccoPanel(false);
	}

	public void CancelChoosingTobacco()
	{
		choosingTobacco = false;
		currentAction = null;
		DisplayTobaccoPanel(false);
		servedHookah = null;
	}

	void DisplayTobaccoPanel(bool toggle)
	{
		tobaccoPanel.gameObject.SetActive(toggle);
	}

	public void AddAction(Action action)
	{
		actions.Enqueue(action);
	}

	public void AddMoveAction(Vector3 dest)
	{
		if (!choosingTobacco)
		{
			actions.Enqueue(new MoveAction(dest));
		}
	}

	public void AddChooseTobaccoAction(Hookah hookah)
	{
		if (!carryingHookah)
		{
			actions.Enqueue(new ChooseTobaccoAction(hookah));
		}
	}

	public void AddTakeHookahAction(Tobacco tobacco)
	{
		actions.Enqueue(new TakeHookahAction(tobacco));
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
	
	// TODO: We must save servedHookah and servedTable
	public void Save(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, selected);
		formatter.Serialize(stream, choosingTobacco);
		formatter.Serialize(stream, carryingHookah);
		formatter.Serialize(stream, currentAction != null);
		//if (currentAction != null) formatter.Serialize(stream, currentAction);

		formatter.Serialize(stream, actions.Count);
		if (actions.Count != 0)
		{
			foreach (Action action in actions)
			{
				formatter.Serialize(stream, actions);
			}
		}

		formatter.Serialize(stream, new SerializedTransform(transform));
	}

	public void Load(BinaryFormatter formatter, FileStream stream)
	{
		selected = (bool)formatter.Deserialize(stream);
		choosingTobacco = (bool)formatter.Deserialize(stream);
		carryingHookah = (bool)formatter.Deserialize(stream);
		bool hasCurrentAction = (bool)formatter.Deserialize(stream);

		//if (hasCurrentAction)
		//{
			//currentAction = (Action)formatter.Deserialize(stream);
			//currentAction.started = false;
		//}

		actions.Clear();
		int actionsCount = (int)formatter.Deserialize(stream);
		if (actionsCount != 0)
		{
			for (int i = 0; i < actionsCount; i++)
			{
				Action action = (Action)formatter.Deserialize(stream);
				actions.Enqueue(action);
			}
		}

		SerializedTransform serializedTransform = (SerializedTransform)formatter.Deserialize(stream);
		TransformDeserializer.Deserialize(serializedTransform, transform);
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

	public bool CarryingHookah
	{
		get { return carryingHookah; }
	}

}
