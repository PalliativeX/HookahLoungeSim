using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.IO;

public class SaveLoadMenu : MonoBehaviour
{
	public bool saveMode;
	bool inMainMenu;

	public TMP_Text menuText;
	public TMP_Text loadSaveButtonText;

	public MenuManager menuManager;
	public TMP_InputField nameInput;
	public RectTransform listContent;
	public SaveLoadItem itemPrefab;

	public SaveLoadManager saveLoadManager;

	void Start()
	{
		inMainMenu = menuManager != null;
	}

	public void Action()
	{
		string path = GetSelectedPath();
		if (path == null) return;

		if (saveMode)
		{
			Save(path);
		}
		else
		{
			if (inMainMenu) LoadFromMenu(path);
			else Load(path);
		}
	}

	public void Open(bool saveMode)
	{
		this.saveMode = saveMode;

		if (saveMode)
		{
			menuText.text = "SAVE MENU";
			loadSaveButtonText.text = "SAVE";
		}
		else
		{
			menuText.text = "LOAD MENU";
			loadSaveButtonText.text = "LOAD";
		}

		FillList();
	}

	void LoadFromMenu(string path)
	{
		if (!File.Exists(path)) return;

		PlayerPrefs.SetString("load path", path);
		PlayerPrefs.Save();

		menuManager.NewGame();
	}

	void Save(string path)
	{
		saveLoadManager.Save(path);

		FillList();
	}

	void Load(string path)
	{
		if (!File.Exists(path)) return;

		//saveLoadManager.Load(path);

		PlayerPrefs.SetString("load path", path);
		PlayerPrefs.Save();

		SceneManager.LoadSceneAsync("MainScene");
		
	}

	string GetSelectedPath()
	{
		string fileName = nameInput.text;
		if (fileName.Length == 0) return null;

		return Path.Combine(Application.persistentDataPath, fileName + ".lounge");
	}

	public void SelectItem(string name)
	{
		nameInput.text = name;
	}

	public void Delete()
	{
		string path = GetSelectedPath();
		if (path == null) return;

		if (File.Exists(path)) File.Delete(path);

		nameInput.text = "";
		FillList();
	}

	void FillList()
	{
		for (int i = 0; i < listContent.childCount; i++)
		{
			Destroy(listContent.GetChild(i).gameObject);
		}

		string[] paths = Directory.GetFiles(Application.persistentDataPath, "*.lounge");
		Array.Sort(paths);

		foreach (string s in paths)
		{
			SaveLoadItem item = Instantiate(itemPrefab);
			item.menu = this;
			item.FileName = Path.GetFileNameWithoutExtension(s);
			item.transform.SetParent(listContent, false);
		}
	}
}
