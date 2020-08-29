using UnityEngine;

public class Table : MonoBehaviour
{
	public bool Occupied { get; set; }
    public bool Reserved { get; set; }

	public Transform sofaPos;
	public Transform hookahPlace;
	public Transform approachPlace;

	public Hookah Hookah { get; set; }
	public Client ClientSitting { get; set; }

	public Vector3 SofaPos
	{
		get { return sofaPos.position; }
	}

	public Vector3 HookahPos
	{
		get { return hookahPlace.position; }
	}
}
