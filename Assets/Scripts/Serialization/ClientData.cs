using UnityEngine;

[System.Serializable]
public class ClientData
{
	public SerializedTransform serializedTransform;
	//public Client client;

	public ClientData(Transform transform, Client client)
	{
		serializedTransform = new SerializedTransform(transform);
		//this.client = client;
	}
}
