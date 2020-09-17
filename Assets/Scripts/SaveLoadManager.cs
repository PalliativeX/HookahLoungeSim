using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveLoadManager : MonoBehaviour
{
	Player player;
	Door door;
	TransparentObjectsController transparentObjController;
	GameMenuManager menuManager;

	private void Start()
	{
		player = FindObjectOfType<Player>();
		transparentObjController = FindObjectOfType<TransparentObjectsController>();
		door = FindObjectOfType<Door>();
		menuManager = FindObjectOfType<GameMenuManager>();

		string loadPath = PlayerPrefs.GetString("load path");
		if (loadPath != null && loadPath != "")
		{
			Load(loadPath);
		}
	}

	// TODO: SAVE AND LOAD DOOR!!
	public void Save(string path)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Create);

		PlayTimer.Instance.Save(formatter, stream);
		player.Save(formatter, stream);
		transparentObjController.Save(formatter, stream);

		stream.Close();
	}

	public void Load(string path)
	{
		BinaryFormatter formatter = new BinaryFormatter();
		FileStream stream = new FileStream(path, FileMode.Open);

		PlayTimer.Instance.Load(formatter, stream);
		menuManager.SwitchGamePause(PlayTimer.Instance.IsPaused);
		player.Load(formatter, stream);
		transparentObjController.Load(formatter, stream);

		stream.Close();

	}
}
