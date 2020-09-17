using UnityEngine;

public class BuildingItemImage : MonoBehaviour
{
	public InteriorItem item;

	BuildingManager buildingManager;

	void Awake()
	{
		buildingManager = FindObjectOfType<BuildingManager>();
	}

	public void Select()
	{
		buildingManager.SelectBuilding(item.prefabName);
	}
}
