using UnityEngine;

public class UnitController : MonoBehaviour
{
	public HookahMaker[] workers;

	public LayerMask workerMask;
	public LayerMask hookahMask;
	public LayerMask tableMask;

	Camera mainCam;

	bool shiftPressed;

    void Start()
    {
		mainCam = Camera.main;
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

	void HandleWorkerActions()
	{
		HookahMaker hookahMaker = GetSelectedWorker();
		if (hookahMaker == null)
			return;

		Ray inputRay = mainCam.ScreenPointToRay(Input.mousePosition);
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
				hookahMaker.AddTakeHookahAction(hookah);
			}
			else if (colliderMask == tableMask)
			{
				Table table = hit.collider.gameObject.GetComponent<Table>();
				//if (hookahMaker.HasServedTable)
				//{
					hookahMaker.AddBringHookahAction(table);
				//}
				//else
				//{
				//	hookahMaker.AddServeTableAction(table);
				//}
			}
			else
			{
				hookahMaker.AddMoveAction(hit.point);
			}
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
