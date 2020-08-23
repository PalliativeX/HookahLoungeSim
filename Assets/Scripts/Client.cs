using UnityEngine;
using UnityEngine.AI;

public enum Status
{
	Waiting,
	Smoking,
	Leaving
}


public class Client : MonoBehaviour
{
	public NavMeshAgent agent;

	public Flavour[] preferredFlavours;
	public FlavourGroup preferredFlavourGroup;
	public Strength  preferredStrength;

	public Status status;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
