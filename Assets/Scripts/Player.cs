using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public class Player : MonoBehaviour
{
	public float startingMoney;
	public WorkingHours workingHours;
	public Transform entry;

	public Table[] tables;
	public Hookah[] hookahs;
	public List<HookahMaker> workers;
	public Tobacco[] tobaccos;

	List<Client> clients;

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

	public void AddWorker(HookahMaker newWorker)
	{
		workers.Add(newWorker);
	}

	public void RemoveWorker(HookahMaker newWorker)
	{
		workers.Remove(newWorker);
	}

	public void Save(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, money);
		formatter.Serialize(stream, rating);
		formatter.Serialize(stream, commentCount);

		formatter.Serialize(stream, clients.Count);
		foreach (Client client in clients)
		{
			client.Save(formatter, stream);
		}
	}

	public void Load(BinaryFormatter formatter, FileStream stream)
	{
		money = (float)formatter.Deserialize(stream);
		rating = (float)formatter.Deserialize(stream);
		commentCount = (int)formatter.Deserialize(stream);

		// NOTE: Loading clients
		foreach (Client client in clients)
		{
			Destroy(client.gameObject);
		}
		clients.Clear();
		int clientsCount = (int)formatter.Deserialize(stream);
		ClientGenerator generator = FindObjectOfType<ClientGenerator>();
		for (int i = 0; i < clientsCount; i++)
		{
			Client client = generator.GenerateClient();
			ClientData data = (ClientData)formatter.Deserialize(stream);
			SerializedTransformExtention.DeserializeTransform(data.serializedTransform, client.transform);
		}
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
