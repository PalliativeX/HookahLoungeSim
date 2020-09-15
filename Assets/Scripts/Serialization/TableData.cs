using UnityEngine;

[System.Serializable]
public class TableData
{
	public SerializedTransform serializedTransform;
	public Table table;

	public TableData(Transform transform, Table table)
	{
		serializedTransform = new SerializedTransform(transform);
		this.table = table;
	}
}
