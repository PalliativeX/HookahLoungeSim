using System.Collections;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Hookah : MonoBehaviour
{
	public string prefabName;

	public Tobacco ContainedTobacco { get; set; }

	public bool Occupied { get; set; }
	public bool Active { get; set; }

	float maxSmokingTime;
	float smokingTimeLeft;

	public void SetActive()
	{
		float smokingTime = ContainedTobacco.brand.smokingTime;
		smokingTimeLeft = smokingTime;
		Active = true;

		StartCoroutine(Smoke());
	}

	IEnumerator Smoke()
	{
		while (smokingTimeLeft > 0f)
		{
			smokingTimeLeft -= PlayTimer.Instance.TimePerFrame();
			yield return null;
		}

		Active = false;
		smokingTimeLeft = 0f;
	}

	public void Save(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, Occupied);
		formatter.Serialize(stream, Active);
		formatter.Serialize(stream, maxSmokingTime);
		formatter.Serialize(stream, smokingTimeLeft);
		formatter.Serialize(stream, ContainedTobacco != null);
		if (ContainedTobacco != null)
		{
			formatter.Serialize(stream, ContainedTobacco);
		}

		SerializedTransform serializedTransform = new SerializedTransform(transform);
		formatter.Serialize(stream, serializedTransform);
	}

	public void Load(BinaryFormatter formatter, FileStream stream)
	{
		Occupied = (bool)formatter.Deserialize(stream);
		Active = (bool)formatter.Deserialize(stream);
		maxSmokingTime = (float)formatter.Deserialize(stream);
		smokingTimeLeft = (float)formatter.Deserialize(stream);

		bool containsTobacco = (bool)formatter.Deserialize(stream);
		if (containsTobacco)
			ContainedTobacco = (Tobacco)formatter.Deserialize(stream);

		SerializedTransform serializedTransform = (SerializedTransform)formatter.Deserialize(stream);
		TransformDeserializer.Deserialize(serializedTransform, transform);
	}
}
