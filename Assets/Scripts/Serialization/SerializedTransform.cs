using UnityEngine;

[System.Serializable]
public class SerializedTransform
{
	public float[] pos = new float[3];
	public float[] rot = new float[4];
	public float[] scale = new float[3];

	public SerializedTransform(Transform transform)
	{
		pos[0] = transform.localPosition.x;
		pos[1] = transform.localPosition.y;
		pos[2] = transform.localPosition.z;

		rot[0] = transform.localRotation.w;
		rot[1] = transform.localRotation.x;
		rot[2] = transform.localRotation.y;
		rot[3] = transform.localRotation.z;

		scale[0] = transform.localScale.x;
		scale[1] = transform.localScale.y;
		scale[2] = transform.localScale.z;
	}
}

public static class SerializedTransformExtention
{
	public static void DeserializeTransform(this SerializedTransform serializedTransform, Transform transform)
	{
		transform.localPosition = new Vector3(serializedTransform.pos[0], serializedTransform.pos[1], serializedTransform.pos[2]);
		transform.localRotation = new Quaternion(serializedTransform.rot[0], serializedTransform.rot[1], serializedTransform.rot[2], serializedTransform.rot[3]);
		transform.localScale = new Vector3(serializedTransform.scale[0], serializedTransform.scale[1], serializedTransform.scale[2]);
	}
}