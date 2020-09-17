using UnityEngine;

[System.Serializable]
public class SerializedTransform
{
	public float[] pos = new float[3];
	public float[] rot = new float[4];
	public float[] scale = new float[3];

	public SerializedTransform(Transform transform)
	{
		pos[0] = transform.position.x;
		pos[1] = transform.position.y;
		pos[2] = transform.position.z;

		rot[0] = transform.rotation.x;
		rot[1] = transform.rotation.y;
		rot[2] = transform.rotation.z;
		rot[3] = transform.rotation.w;

		//scale[0] = transform.localScale.x;
		//scale[1] = transform.localScale.y;
		//scale[2] = transform.localScale.z;

		// NB: This approach can be erroneous, so keep that in mind!
		scale[0] = transform.lossyScale.x;
		scale[1] = transform.lossyScale.y;
		scale[2] = transform.lossyScale.z;
	}
}

public static class TransformDeserializer
{
	public static void Deserialize(this SerializedTransform serializedTransform, Transform transform)
	{
		transform.position = new Vector3(serializedTransform.pos[0], serializedTransform.pos[1], serializedTransform.pos[2]);
		transform.rotation = new Quaternion(serializedTransform.rot[0], serializedTransform.rot[1], serializedTransform.rot[2], serializedTransform.rot[3]);
		transform.localScale = new Vector3(serializedTransform.scale[0], serializedTransform.scale[1], serializedTransform.scale[2]);
	}
}