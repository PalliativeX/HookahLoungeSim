using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
	Player player;
	Door door;
	TransparentObjectsController transparentObjController;

	private void Start()
	{
		player = FindObjectOfType<Player>();
		transparentObjController = FindObjectOfType<TransparentObjectsController>();
		door = FindObjectOfType<Door>();

		string loadPath = PlayerPrefs.GetString("load path");
		if (loadPath != null && loadPath != "")
		{
			Load(loadPath);
		}
	}

	public void Save(string path)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);

		PlayTimer.Instance.Save(formatter, stream);
		player.Save(formatter, stream);
		transparentObjController.Save(formatter, stream);

		stream.Close();

		/*using (BinaryFormatter formatter = new BinaryFormatter(File.Open(path, FileMode.Create)))
		//{
			PlayTimer.Instance.Save(writer);
			player.Save(writer);
			transparentObjController.Save(writer);
			door.Save(writer);
		//}*/
	}

	public void Load(string path)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Open);

		PlayTimer.Instance.Load(formatter, stream);
		player.Load(formatter, stream);
		transparentObjController.Load(formatter, stream);

		stream.Close();

		/*using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
		{
			PlayTimer.Instance.Load(reader);
			player.Load(reader);
			transparentObjController.Load(reader);
			door.Load(reader);
		}*/
	}
}
