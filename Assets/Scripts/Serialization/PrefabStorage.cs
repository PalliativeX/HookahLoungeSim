using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
	public Hookah[] hookahPrefabs;
	public HookahMaker[] hookahMakerPrefabs;
	public Client[] clientPrefabs;
	public Table[] tablePrefabs;

	public static PrefabStorage Instance { get; private set; }

	void Awake()
	{
		if (Instance != null && Instance != this)
			Destroy(gameObject);
		else
			Instance = this;
	}

	public Hookah GetHookahByName(string name)
	{
		foreach (Hookah hookah in hookahPrefabs)
		{
			if (hookah.prefabName == name) return hookah;
		}

		return null;
	}

	public HookahMaker GetHookahMakerByName(string name)
	{
		foreach (HookahMaker hookahMaker in hookahMakerPrefabs)
		{
			if (hookahMaker.prefabName == name) return hookahMaker;
		}

		return null;
	}

	public Client GetClientByName(string name)
	{
		foreach (Client client in clientPrefabs)
		{
			if (client.prefabName == name) return client;
		}

		return null;
	}

	public Table GetTableByName(string name)
	{
		foreach (Table table in tablePrefabs)
		{
			if (table.prefabName == name) return table;
		}

		return null;
	}

}
