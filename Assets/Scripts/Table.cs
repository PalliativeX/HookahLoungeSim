using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Table : MonoBehaviour
{
	public bool Occupied { get; set; }
    public bool Reserved { get; set; }

	public Transform sofaPos;
	public Transform hookahPlace;
	public Transform approachPlace;

	// TODO: How to serialize that??
	public Hookah Hookah { get; set; }
	public Client ClientSitting { get; set; }

	public Vector3 SofaPos
	{
		get { return sofaPos.position; }
	}

	public Vector3 HookahPos
	{
		get { return hookahPlace.position; }
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write(Occupied);
		writer.Write(Reserved);

		BinaryFormatter formatter = new BinaryFormatter();

		TableData tableData = new TableData(transform, this);
		formatter.Serialize(writer.BaseStream, tableData);
	}

	public void Read(BinaryReader reader)
	{
		Occupied = reader.ReadBoolean();
		Reserved = reader.ReadBoolean();
	}
}


