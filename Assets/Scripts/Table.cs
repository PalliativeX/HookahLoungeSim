using UnityEngine;

public class Table : MonoBehaviour
{
	public bool Occupied { get; set; }
    public bool Reserved { get; set; }

	public Transform sofaPos;

	public Hookah Hookah { get; set; }
	public Client ClientSitting { get; set; }
}
