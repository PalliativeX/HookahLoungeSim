using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class TransparentObjectsController : MonoBehaviour
{
	public Renderer[] wallRenderers;

	public Material transparentWallMat;
	public Material opaqueWallMat;

	public KeyCode changeMat = KeyCode.T;

	bool currentlyTransparent;

	private void Start()
	{
		currentlyTransparent = true;
		ChangeTransparency(currentlyTransparent);
	}

	private void Update()
	{
		if (Input.GetKeyDown(changeMat))
		{
			ChangeTransparency(!currentlyTransparent);
		}
	}

	public void ToggleTransparentcy()
	{
		ChangeTransparency(!currentlyTransparent);
	}

	public void ChangeTransparency(bool transparent)
	{
		currentlyTransparent = transparent;
		if (!transparent)
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
		ChangeTransparency(currentlyTransparent);
	}

}
