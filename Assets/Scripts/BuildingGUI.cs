using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingGUI : MonoBehaviour
{
	BuildingManager buildingManager;
	public GameObject gui;

	public Image image;

	bool isActive;

	private void Start()
	{
		buildingManager = FindObjectOfType<BuildingManager>();
		isActive = false;
		gui.SetActive(false);

		image.sprite = buildingManager.items[0].icon;
	}

	public void SwitchGui()
	{
		isActive = !isActive;
		gui.SetActive(isActive);
	}

	public void SelectChair()
	{
		buildingManager.SelectBuilding(0);
	}
}
