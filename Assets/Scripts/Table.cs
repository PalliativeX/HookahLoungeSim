using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public class Table : MonoBehaviour
{
	public string prefabName;

	public bool Occupied { get; set; }
    public bool Reserved { get; set; }

	public Transform sofaPos;
	public Transform hookahPlace;
	public Transform approachPlace;

	public Hookah Hookah { get; set; }
	public Client ClientSitting { get; set; }

	[HideInInspector]
	public TableData tableData;

	public Vector3 SofaPos
	{
		get { return sofaPos.position; }
	}

	public Vector3 HookahPos
	{
		get { return hookahPlace.position; }
	}

	public void Save(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, Occupied);
		formatter.Serialize(stream, Reserved);

		formatter.Serialize(stream, new SerializedTransform(transform));
	}

	public void Load(BinaryFormatter formatter, FileStream stream)
	{
		Occupied = (bool)formatter.Deserialize(stream);
		Reserved = (bool)formatter.Deserialize(stream);

		SerializedTransform serializedTransform = (SerializedTransform)formatter.Deserialize(stream);
		TransformDeserializer.Deserialize(serializedTransform, transform);
	}
}


