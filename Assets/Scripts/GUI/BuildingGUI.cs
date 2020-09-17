using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingGUI : MonoBehaviour
{
	BuildingManager buildingManager;
	public GameObject gui;
	public RectTransform scrollviewContent;

	public BuildingItemImage[] itemImages;

	bool isActive;

	private void Start()
	{
		buildingManager = FindObjectOfType<BuildingManager>();
		isActive = false;
		gui.SetActive(false);
	}

	public void SwitchGui()
	{
		isActive = !isActive;
		gui.SetActive(isActive);

		if (isActive) FillScrollview();
	}

	public void SelectChair()
	{
		buildingManager.SelectBuilding(0);
	}

	public void FillScrollview()
	{
		for (int i = 0; i < scrollviewContent.childCount; i++)
		{
			Destroy(scrollviewContent.GetChild(i).gameObject);
		}

		for (int i = 0; i < buildingManager.items.Length; i++)
		{
			Image item = Instantiate(itemImages[i].GetComponent<Image>());
			item.transform.SetParent(scrollviewContent, false);
		}
	}
}
