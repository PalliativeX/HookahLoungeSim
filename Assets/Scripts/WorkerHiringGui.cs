using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WorkerHiringGui : MonoBehaviour
{
	WorkerHiringManager hiringManager;

	HookahMaker currentHookahMaker;

	public GameObject panel;
	public GameObject workerHiringInfo;
	public TMP_Text workerName;
	public TMP_Text workerCost;
	public TMP_Text noWorkersAvailableText;
	public Image workerIcon;

	void Start()
	{
		hiringManager = FindObjectOfType<WorkerHiringManager>();

		currentHookahMaker = hiringManager.CurrentHookahMaker;
		UpdateCurrentWorkerDisplay();
		panel.SetActive(false);
	}

	public void Next()
	{
		currentHookahMaker = hiringManager.AdvanseWorkersList(1);
		UpdateCurrentWorkerDisplay();
	}

	public void Prev()
	{
		currentHookahMaker = hiringManager.AdvanseWorkersList(-1);
		UpdateCurrentWorkerDisplay();
	}

	public void Hire()
	{
		HookahMaker hookahMaker = hiringManager.CreateHookahMaker();

		currentHookahMaker = hiringManager.CurrentHookahMaker;
		UpdateCurrentWorkerDisplay();
	}

	public void UpdateCurrentWorkerDisplay()
	{
		if (currentHookahMaker == null)
		{
			workerHiringInfo.SetActive(false);
			noWorkersAvailableText.gameObject.SetActive(true);
		}
		else
		{
			workerHiringInfo.SetActive(true);
			noWorkersAvailableText.gameObject.SetActive(false);

			workerName.text = currentHookahMaker.name;
			workerCost.text = currentHookahMaker.cost + "$";
			workerIcon.sprite = currentHookahMaker.icon;
		}
	}

	public void ClosePanel()
	{
		panel.SetActive(false);
	}

	public void Activate()
	{
		panel.SetActive(true);
	}

}
