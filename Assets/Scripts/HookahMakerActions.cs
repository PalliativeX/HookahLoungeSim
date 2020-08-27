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

public class TakeHookahAction : Action
{
	public Hookah hookah;

	public TakeHookahAction(Hookah hookah)
	{
		this.hookah = hookah;
	}
}

public class BringHookahAction : Action
{
	public Table table;

	public BringHookahAction(Table table)
	{
		this.table = table;
	}
}

public class ServeTableAction : Action
{
	public Table table;

	public ServeTableAction(Table table)
	{
		this.table = table;
	}
}