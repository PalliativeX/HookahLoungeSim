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
	public Tobacco tobacco;

	public TakeHookahAction(Tobacco tobacco)
	{
		this.tobacco = tobacco;
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

public class ChooseTobaccoAction : Action
{
	public Hookah hookah;

	public ChooseTobaccoAction(Hookah hookah)
	{
		this.hookah = hookah;
	}
}