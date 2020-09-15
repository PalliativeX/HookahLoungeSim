using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class TransparentObjectsController : MonoBehaviour
{
	public Renderer[] wallRenderers;

	public Material transparentWallMat;
	public Material opaqueWallMat;

	public KeyCode changeMat = KeyCode.T;

	bool currentlyTransparent = true;

	private void Start()
	{
		ChangeTransparency();
	}

	private void Update()
	{
		if (Input.GetKeyDown(changeMat))
		{
			ChangeTransparency();
		}
	}

	public void ChangeTransparency()
	{
		currentlyTransparent = !currentlyTransparent;
		if (currentlyTransparent)
		{
			ChangeMat(opaqueWallMat);
			TurnColliders(collidersOn: true);
		}
		else
		{
			ChangeMat(transparentWallMat);
			TurnColliders(collidersOn: false);
		}
	}


	void ChangeMat(Material mat)
	{
		foreach (Renderer renderer in wallRenderers)
		{
			renderer.material = mat;
		}
	}

	void TurnColliders(bool collidersOn)
	{
		foreach (Renderer renderer in wallRenderers)
		{
			if (renderer.TryGetComponent(out Collider collider))
			{
				collider.enabled = collidersOn;
			}
		}
	}

	public void Save(BinaryFormatter formatter, FileStream stream)
	{
		formatter.Serialize(stream, currentlyTransparent);
	}

	public void Load(BinaryFormatter formatter, FileStream stream)
	{
		currentlyTransparent = (bool)formatter.Deserialize(stream);
	}

}
