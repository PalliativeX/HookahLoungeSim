using UnityEngine;
using System;

[Serializable]
public class Action
{
	public bool started;
}

[Serializable]
public class MoveAction : Action
{
	public Vector3 dest;

	public MoveAction(Vector3 dest)
	{
		this.dest = dest;
	}
}

[Serializable]
public class TakeHookahAction : Action
{
	public Tobacco tobacco;

	public TakeHookahAction(Tobacco tobacco)
	{
		this.tobacco = tobacco;
	}
}

[Serializable]
public class BringHookahAction : Action
{
	public Table table;

	public BringHookahAction(Table table)
	{
		this.table = table;
	}
}

[Serializable]
public class ChooseTobaccoAction : Action
{
	public Hookah hookah;

	public ChooseTobaccoAction(Hookah hookah)
	{
		this.hookah = hookah;
	}
}