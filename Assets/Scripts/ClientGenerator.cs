using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class ClientGenerator : MonoBehaviour
{
	public float minWaitMinutes;
	public float maxWaitMinutes;
	public Client[] clientPrefabs;
	public Door door;
	public Transform spawnPlace;
	public Player player;

	bool waiting;

	void Update()
	{
		if (!waiting && player.WorkStatus == WorkStatus.Open)
		{
			StartCoroutine(GenerateClientAfterDelay());
		}
	}

	IEnumerator GenerateClientAfterDelay()
	{
		waiting = true;

		yield return StartCoroutine(Wait());

		GenerateClient();

		waiting = false;
	}

	IEnumerator Wait()
	{
		float popularity = CalculatePopularity();
		float timeToWait = Random.Range(minWaitMinutes, maxWaitMinutes) / popularity;
		while (timeToWait > 0f)
		{
			timeToWait -= PlayTimer.Instance.TimePerFrame();
			yield return null;
		}
	}

	float CalculatePopularity()
	{
		float popularitySpawnIncrease = 1;

		popularitySpawnIncrease += player.Rating / 5f;

		return popularitySpawnIncrease;
	}

	public Client GenerateClient()
	{
		Client newClient = Instantiate(clientPrefabs[Random.Range(0, clientPrefabs.Length)]);
		newClient.transform.position = spawnPlace.position;
		newClient.Enter();
		player.AddClient(newClient);

		return newClient;
	}

	public Client GenerateClient(Client clientPrefab)
	{
		Client newClient = Instantiate(clientPrefab);
		newClient.transform.position = spawnPlace.position;
		player.AddClient(newClient);

		return newClient;
	}

}
