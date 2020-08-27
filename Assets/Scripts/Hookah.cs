using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hookah : MonoBehaviour
{
	public float MaxSmokingTime { get; set; }
	public bool Occupied { get; set; }
	public Client Client { get; set; }

	float smokingTimeLeft;

	// TODO: Think through
	public void SetActive(float smokingTime)
	{

	}
}
