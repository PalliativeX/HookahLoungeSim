﻿using UnityEngine;

public class UnitController : MonoBehaviour
{
	Player player;

	public LayerMask workerMask;
	public LayerMask hookahMask;
	public LayerMask tableMask;

	Camera mainCamera;

	bool shiftPressed;

    void Start()
    {
		mainCamera = Camera.main;
		player = GetComponent<Player>();
    }

    void Update()
    {
		if (Input.GetKey(KeyCode.LeftShift))
		{
			shiftPressed = true;
		}

		if (Input.GetMouseButtonDown(0))
		{
			HandleWorkerSelection();
		}
		if (Input.GetMouseButtonDown(1))
		{
			HandleWorkerActions();
		}
	}

	void HandleWorkerSelection()
	{
		DeselectWorkers();

		Ray inputRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(inputRay, out RaycastHit hit))
		{
			if (hit.collider != null)
			{
				LayerMask colliderMask = 1 << hit.collider.gameObject.layer;
				if (colliderMask == workerMask)
				{
					HookahMaker hookahMaker = hit.collider.gameObject.GetComponent<HookahMaker>();
					hookahMaker.Selected = true;
				}
			}
		}
	}

	void HandleWorkerActions()
	{
		HookahMaker hookahMaker = GetSelectedWorker();
		if (hookahMaker == null)
			return;

		Ray inputRay = mainCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(inputRay, out RaycastHit hit))
		{
			LayerMask colliderMask = 1 << hit.collider.gameObject.layer;

			if (!shiftPressed)
			{
				hookahMaker.ClearActions();
			}

			if (colliderMask == hookahMask)
			{
				Hookah hookah = hit.collider.gameObject.GetComponent<Hookah>();
				hookahMaker.AddChooseTobaccoAction(hookah);
			}
			else if (colliderMask == tableMask)
			{
				Table table = hit.collider.gameObject.GetComponent<Table>();
				hookahMaker.AddBringHookahAction(table);
			}
			else
			{
				hookahMaker.AddMoveAction(hit.point);
			}
		}
	}

	void DeselectWorkers()
	{
		foreach (HookahMaker hookahMaker in player.workers)
		{
			hookahMaker.Selected = false;
		}
	}

	HookahMaker GetSelectedWorker()
	{
		foreach (HookahMaker hookahMaker in player.workers)
		{
			if (hookahMaker.Selected)
				return hookahMaker;
		}

		return null;
	}
}
