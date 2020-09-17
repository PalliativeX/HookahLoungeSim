using UnityEngine;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;

public class PlayerDataLoader : MonoBehaviour
{
	Player player;

	private void Start()
	{
		player = FindObjectOfType<Player>();
	}

	public void Save(BinaryFormatter formatter, FileStream stream)
	{
		SerializeClients(formatter, stream);
		SerializeTables(formatter, stream);
		SerializeHookahs(formatter, stream);
		SerializeHookahMakers(formatter, stream);
	}

	void SerializeClients(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, player.clients.Count);
		foreach (Client client in player.clients)
		{
			formatter.Serialize(stream, client.prefabName);
			client.Save(formatter, stream);

			ClientData clientData = new ClientData();

			if (client.occupiedTable == null)
			{
				clientData.tableIndex = -1;
			}
			else
			{
				for (int i = 0; i < player.tables.Count; i++)
				{
					if (client.occupiedTable == player.tables[i])
						clientData.tableIndex = i;
				}
			}

			if (client.smokedHookah == null)
			{
				clientData.hookahIndex = -1;
			}
			else
			{
				for (int i = 0; i < player.hookahs.Count; i++)
				{
					if (client.smokedHookah == player.hookahs[i])
						clientData.hookahIndex = i;
				}
			}

			formatter.Serialize(stream, clientData);
		}
	}

	void SerializeTables(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, player.tables.Count);
		foreach (Table table in player.tables)
		{
			formatter.Serialize(stream, table.prefabName);
			table.Save(formatter, stream);

			TableData tableData = new TableData();

			if (table.Hookah == null)
			{
				tableData.hookahIndex = -1;
			}
			else
			{
				for (int i = 0; i < player.hookahs.Count; i++)
				{
					if (table.Hookah == player.hookahs[i])
						tableData.hookahIndex = i;
				}
			}

			if (table.ClientSitting == null)
			{
				tableData.clientIndex = -1;
			}
			else
			{
				for (int i = 0; i < player.clients.Count; i++)
				{
					if (table.ClientSitting == player.clients[i])
						tableData.clientIndex = i;
				}
			}

			formatter.Serialize(stream, tableData);
		}
	}

	void SerializeHookahs(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, player.hookahs.Count);
		foreach (Hookah hookah in player.hookahs)
		{
			formatter.Serialize(stream, hookah.prefabName);
			hookah.Save(formatter, stream);
		}
	}

	void SerializeHookahMakers(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, player.workers.Count);
		foreach (HookahMaker worker in player.workers)
		{
			formatter.Serialize(stream, worker.prefabName);
			worker.Save(formatter, stream);

			HookahMakerData hookahMakerData = new HookahMakerData();

			if (worker.servedHookah == null)
			{
				hookahMakerData.hookahIndex = -1;
			}
			else
			{
				for (int i = 0; i < player.hookahs.Count; i++)
				{
					if (worker.servedHookah == player.hookahs[i])
						hookahMakerData.hookahIndex = i;
				}
			}

			if (worker.servedTable == null)
			{
				hookahMakerData.tableIndex = -1;
			}
			else
			{
				for (int i = 0; i < player.tables.Count; i++)
				{
					if (worker.servedTable == player.tables[i])
						hookahMakerData.tableIndex = i;
				}
			}

			formatter.Serialize(stream, hookahMakerData);
		}

	}

	public void Load(BinaryFormatter formatter, FileStream stream)
	{
		// NOTE: Loading clients
		foreach (Client client in player.clients) Destroy(client.gameObject);
		player.clients.Clear();
		int clientsCount = (int)formatter.Deserialize(stream);
		ClientGenerator generator = FindObjectOfType<ClientGenerator>();
		for (int i = 0; i < clientsCount; i++)
		{
			string prefabName = (string)formatter.Deserialize(stream);
			Client clientPrefab = PrefabStorage.Instance.GetClientByName(prefabName);

			Client client = Instantiate(clientPrefab);
			client.Load(formatter, stream);
			client.prefabName = prefabName;
			player.AddClient(client);

			ClientData clientData = (ClientData)formatter.Deserialize(stream);

			client.clientData = clientData;
		}

		// NOTE: Loading tables
		foreach (Table table in player.tables) Destroy(table.gameObject);
		int tablesCount = (int)formatter.Deserialize(stream);
		player.tables.Clear();
		for (int i = 0; i < tablesCount; i++)
		{
			string prefabName = (string)formatter.Deserialize(stream);
			Table tablePrefab = PrefabStorage.Instance.GetTableByName(prefabName);

			Table table = Instantiate(tablePrefab);
			table.Load(formatter, stream);
			table.prefabName = prefabName;
			player.tables.Add(table);

			TableData tableData = (TableData)formatter.Deserialize(stream);

			table.tableData = tableData;
		}

		// NOTE: Loading hookahs
		foreach (Hookah hookah in player.hookahs) Destroy(hookah.gameObject);
		int hookahsCount = (int)formatter.Deserialize(stream);
		player.hookahs.Clear();
		for (int i = 0; i < hookahsCount; i++)
		{
			string prefabName = (string)formatter.Deserialize(stream);
			Hookah hookahPrefab = PrefabStorage.Instance.GetHookahByName(prefabName);

			Hookah hookah = Instantiate(hookahPrefab);
			hookah.Load(formatter, stream);
			hookah.prefabName = prefabName;
			player.hookahs.Add(hookah);
		}

		// NOTE: Loading hookah makers
		foreach (HookahMaker worker in player.workers) Destroy(worker.gameObject);
		int workersCount = (int)formatter.Deserialize(stream);
		player.workers.Clear();
		for (int i = 0; i < workersCount; i++)
		{
			string prefabName = (string)formatter.Deserialize(stream);
			HookahMaker hookahMakerPrefab = PrefabStorage.Instance.GetHookahMakerByName(prefabName);

			HookahMaker worker = Instantiate(hookahMakerPrefab);
			worker.Load(formatter, stream);
			worker.prefabName = prefabName;
			player.workers.Add(worker);

			HookahMakerData hookahMakerData = (HookahMakerData)formatter.Deserialize(stream);

			worker.hookahMakerData= hookahMakerData;
		}


		RecreateClientLinks(formatter, stream);
		RecreateTableLinks(formatter, stream);
		RecreateHookahMakerLinks(formatter, stream);
	}

	void RecreateClientLinks(BinaryFormatter formatter, FileStream stream)
	{
		for (int i = 0; i < player.clients.Count; i++)
		{
			Client current = player.clients[i];

			if (current.clientData.hookahIndex != -1)
				current.smokedHookah = player.hookahs[current.clientData.hookahIndex];
			if (current.clientData.tableIndex != -1)
				current.occupiedTable = player.tables[current.clientData.tableIndex];
		}
	}

	void RecreateTableLinks(BinaryFormatter formatter, FileStream stream)
	{
		for (int i = 0; i < player.tables.Count; i++)
		{
			Table current = player.tables[i];

			if (current.tableData.hookahIndex != -1)
				current.Hookah = player.hookahs[current.tableData.hookahIndex];
			if (current.tableData.clientIndex != -1)
				current.ClientSitting = player.clients[current.tableData.clientIndex];

			if (current.Hookah != null)
			{
				//current.Hookah.transform.SetParent(current.transform);
				current.Hookah.transform.position = current.HookahPos;
			}
		}
	}

	void RecreateHookahMakerLinks(BinaryFormatter formatter, FileStream stream)
	{
		for (int i = 0; i < player.workers.Count; i++)
		{
			HookahMaker current = player.workers[i];

			if (current.hookahMakerData.hookahIndex != -1)
				current.servedHookah = player.hookahs[current.hookahMakerData.hookahIndex];
			if (current.hookahMakerData.tableIndex != -1)
				current.servedTable = player.tables[current.hookahMakerData.tableIndex];

			if (current.servedHookah != null && current.CarryingHookah)
			{
				current.servedHookah.GetComponent<Collider>().enabled = false;
				current.servedHookah.transform.SetParent(current.transform);
				current.servedHookah.transform.position = current.hookahCarryPos.position;
			}
		}
	}

}

[Serializable]
public class ClientData
{
	public int hookahIndex;
	public int tableIndex;

	public ClientData(int hookahIndex, int tableIndex)
	{
		this.hookahIndex = hookahIndex;
		this.tableIndex = tableIndex;
	}

	public ClientData() { }
}

[Serializable]
public class HookahMakerData
{
	public int hookahIndex;
	public int tableIndex;

	public HookahMakerData(int hookahIndex, int tableIndex)
	{
		this.hookahIndex = hookahIndex;
		this.tableIndex = tableIndex;
	}

	public HookahMakerData() { }
}

[Serializable]
public class TableData
{
	public int hookahIndex;
	public int clientIndex;

	public TableData(int hookahIndex, int clientIndex)
	{
		this.hookahIndex = hookahIndex;
		this.clientIndex = clientIndex;
	}

	public TableData() { }
}
