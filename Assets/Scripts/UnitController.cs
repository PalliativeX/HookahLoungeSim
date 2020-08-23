using UnityEngine;

public class UnitController : MonoBehaviour
{
	public HookahMaker[] workers;

	public LayerMask workerMask;

	Camera mainCam;

    void Start()
    {
		mainCam = Camera.main;
    }

    void Update()
    {
		if (Input.GetMouseButtonDown(0))
		{
			HandleWorkerSelection();
		}
		if (Input.GetMouseButtonDown(1))
		{
			HandleWorkerMovement();
		}
	}

	void HandleWorkerSelection()
	{
		DeselectWorkers();

		Ray inputRay = mainCam.ScreenPointToRay(Input.mousePosition);
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

	void HandleWorkerMovement()
	{
		HookahMaker hookahMaker = GetSelectedWorker();
		if (hookahMaker == null)
			return;

		Ray inputRay = mainCam.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(inputRay, out RaycastHit hit))
		{
			LayerMask colliderMask = 1 << hit.collider.gameObject.layer;

			hookahMaker.ClearActions();
			hookahMaker.AddMoveAction(hit.point);
		}

	}

	void DeselectWorkers()
	{
		foreach (HookahMaker hookahMaker in workers)
		{
			hookahMaker.Selected = false;
		}
	}

	HookahMaker GetSelectedWorker()
	{
		foreach (HookahMaker hookahMaker in workers)
		{
			if (hookahMaker.Selected)
				return hookahMaker;
		}

		return null;
	}
}
