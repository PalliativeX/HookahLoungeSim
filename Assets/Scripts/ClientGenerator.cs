using UnityEngine;
using System.Collections;

// TODO: Rename and refactor so that it's more readable
public class ClientGenerator : MonoBehaviour
{
	public Client[] clientPrefabs;
	public Door door;

	public Transform spawnPlace;
	public Player player;
	public float minWaitMinutes;
	public float maxWaitMinutes;

	float timePassed;

	bool waiting;

	void Update()
	{
		if (!waiting)
		{
			StartCoroutine(WaitAndGenerate());
		}
	}

	IEnumerator WaitAndGenerate()
	{
		waiting = true;
		yield return new WaitForSeconds(Random.Range(minWaitMinutes, maxWaitMinutes) / player.GetPlaySpeed());

		yield return StartCoroutine(GenerateClientAfterWait(2f));
		waiting = false;
	}

	// NOTE: We wait before the door opens
	IEnumerator GenerateClientAfterWait(float seconds)
	{
		door.Open();

		yield return new WaitForSeconds(seconds / player.GetPlaySpeed());

		GenerateClient();
	}

	void CalculatePopularity()
	{
		float popularity = player.Rating;
	}

	void GenerateClient()
	{
		Client newClient = Instantiate(clientPrefabs[Random.Range(0, clientPrefabs.Length)]);
		newClient.transform.position = spawnPlace.position;
	}

}
