using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadItem : MonoBehaviour
{
	public SaveLoadMenu menu;

	string fileName;

	public string FileName
	{
		get { return fileName; }
		set {
			fileName = value;
			transform.GetChild(0).GetComponent<TMP_Text>().text = value;
		}
	}

	private void Awake()
	{
		menu = FindObjectOfType<SaveLoadMenu>();
	}

	public void Select()
	{
		menu.SelectItem(fileName);
	}
}
