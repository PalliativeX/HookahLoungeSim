using UnityEngine;

public class Action
{
	public bool started;
}

public class MoveAction : Action
{
	public Vector3 dest;

	public MoveAction(Vector3 dest)
	{
		this.dest = dest;
	}
}