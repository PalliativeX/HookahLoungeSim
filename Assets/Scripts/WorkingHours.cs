using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WorkingHours
{
	public int beginning;
	public int ending;

	public WorkingHours(int beginning, int ending)
	{
		this.beginning = beginning;
		this.ending = ending;
	}
}