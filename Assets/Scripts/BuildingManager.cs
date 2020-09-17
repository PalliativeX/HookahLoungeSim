using UnityEngine;

public class BuildingManager : MonoBehaviour
{
	public Vector3 loungeStart;
	public float loungeWidth, loungeLength;

	public InteriorItem[] items;

	GameObject currentBuilding;
	int currentIndex;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && currentBuilding != null)
		{
			Destroy(currentBuilding.gameObject);
			currentBuilding = null;
		}

		if (currentBuilding != null)
		{
			Vector3 origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Vector3 direction = Camera.main.transform.position;


			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit))
			{
				currentBuilding.transform.position = new Vector3(hit.point.x, 0, hit.point.z) + Vector3.left*2f;
				ValidatePosition();
			}
		}
	}

	public void SelectBuilding(string prefabName)
	{
		for (int i = 0; i < items.Length; i++)
		{
			if (items[i].prefabName == prefabName)
				SelectBuilding(i);
		}
	}

	public void SelectBuilding(int index)
	{
		currentIndex = index;
		CreateBuildingHologram();
	}

	void ValidatePosition()
	{

	}

	void CreateBuildingHologram()
	{
		GameObject building = Instantiate(items[currentIndex].gameObject, transform);
		UpdateColor(building);
		currentBuilding = building;
	}

	void UpdateColor(GameObject obj)
	{
		if (obj.GetComponent<Renderer>() != null)
			obj.GetComponent<Renderer>().material.color = new Color(0.1f, 0.9f, 0.1f, 0.4f);
		foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
		{
			if (renderer != null)
				renderer.material.color = new Color(0.1f, 0.9f, 0.1f, 0.4f);
		}
	}

	/*
	// NOTE: Building a 2d grid for the lounge
	private void OnDrawGizmos()
	{
		for (int x = -18; x < 60; x+=2)
		{
			for (int z = -18; z < 60; z+=2)
			{
				Gizmos.DrawCube(transform.position + new Vector3(x, 0, z), new Vector3(2, 0.2f, 2));
			}
		}
	}*/

}
