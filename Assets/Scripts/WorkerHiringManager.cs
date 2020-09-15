using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerHiringManager : MonoBehaviour
{
	public HookahMaker[] hookahMakerPrefabs;
	public Transform entry;
	public Transform workerMovePos;
	//public string[] firstNames;
	//public string[] lastNames;

	Player player;
	int currentWorkerIndex;
	List<HookahMaker> availableHookahMakers; // NOTE: Available for hiring

	private void Awake()
	{
		player = FindObjectOfType<Player>();
		availableHookahMakers = new List<HookahMaker>();

		UpdateAvailableHookahMakers();
		currentWorkerIndex = 0;
	}

	public HookahMaker AdvanseWorkersList(int offset)
	{
		currentWorkerIndex += offset;

		int count = availableHookahMakers.Count;

		if (count == 0) return null;

		if (currentWorkerIndex < 0)
		{
			currentWorkerIndex = count - 1;
		}
		else if (currentWorkerIndex > count - 1)
		{
			currentWorkerIndex = 0;
		}

		return availableHookahMakers[currentWorkerIndex];
	}

	private void UpdateAvailableHookahMakers()
	{
		availableHookahMakers.Clear();

		foreach (HookahMaker hookahMaker in hookahMakerPrefabs)
		{
			availableHookahMakers.Add(hookahMaker);
		}
	}

	public HookahMaker CreateHookahMaker()
	{
		HookahMaker newWorker = Instantiate(availableHookahMakers[currentWorkerIndex], entry.position, Quaternion.identity, player.transform);
		player.AddWorker(newWorker);

		newWorker.AddMoveAction(workerMovePos.position);

		availableHookahMakers.RemoveAt(currentWorkerIndex);
		currentWorkerIndex = 0;

		return newWorker;
	}

	public HookahMaker GetCurrentHookahMaker()
	{
		return availableHookahMakers[currentWorkerIndex];
	}

	public HookahMaker CurrentHookahMaker
	{
		get {
			if (availableHookahMakers.Count > currentWorkerIndex)
				return availableHookahMakers[currentWorkerIndex];
			else
				return null;
		}
	}

	public int CurrentHookahMakerIndex
	{
		get { return currentWorkerIndex; }
	}
}
