using UnityEngine;
using System.Collections.Generic;
using System.Linq;



public class Player : MonoBehaviour
{
	public float startingMoney = 1000f;
	public WorkingHours workingHours;

	public Table[] tables;
	public Hookah[] hookahs;
	public HookahMaker[] workers;
	public Tobacco[] tobaccos;

	List<Client> clients;

	public Transform entry;

	PlayerGUI playerGUI;
	float money;
	float rating;
	int commentCount;
	WorkStatus workStatus;

	void Awake()
	{
		playerGUI = GetComponent<PlayerGUI>();
		clients = new List<Client>();
		workStatus = WorkStatus.Open;
		money = startingMoney;
	}

	private void Update()
	{
		WorkStatus = PlayTimer.Instance.GetStatus(workingHours);
	}

	public bool HasFreeTables()
	{
		return tables.Any(table => !table.Occupied);
	}

	public Table GetFreeTable()
	{
		return tables.First(table => !table.Occupied);
	}

	public bool HasFreeHookahs()
	{
		return hookahs.Any(hookah => !hookah.Occupied);
	}

	public Hookah GetFreeHookah()
	{
		return hookahs.First(hookah => !hookah.Occupied);
	}

	public bool HasFlavour(Flavour specifiedFlavour)
	{
		return tobaccos.Any(tobacco => specifiedFlavour == tobacco.flavour);
	}

	public Flavour GetRandomFlavour()
	{
		return tobaccos[Random.Range(0, tobaccos.Length - 1)].flavour;
	}

	public Tobacco GetRandomTobacco()
	{
		return tobaccos[Random.Range(0, tobaccos.Length - 1)];
	}

	public Tobacco[] GetTobaccos()
	{
		return tobaccos;
	}

	public void AddClient(Client newClient)
	{
		clients.Add(newClient);
	}

	public void RemoveClient(Client client)
	{
		clients.Remove(client);
	}

	public float Money
	{
		get { return money; }
		set {
			money = value;
			playerGUI.UpdateMoneyText();
		}
	}

	public WorkStatus WorkStatus
	{
		get { return workStatus; }
		set {
			workStatus = value;
			playerGUI.UpdateStatusSignText();
		}
	}

	public float Rating
	{
		get { return rating; }
		set {
			commentCount++;
			rating = (rating + value) / commentCount;
			playerGUI.UpdateRatingText();
		}
	}

}

public enum WorkStatus
{
	Open, Closed
}
