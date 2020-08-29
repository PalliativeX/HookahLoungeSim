using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
	private SpriteRenderer backgroundSpriteRenderer;
	private TextMeshPro textMeshPro;

	public static Transform prefab;

	private void Awake()
	{
		backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
		textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
	}

	public void SetText(string text)
	{
		SetUp(text);
	}

	void SetUp(string text)
	{
		textMeshPro.SetText(text);
		textMeshPro.ForceMeshUpdate();
		Vector2 textSize = textMeshPro.GetRenderedValues(false);
		Vector2 padding = new Vector2(4f, 2f);
		backgroundSpriteRenderer.size = textSize + padding;
	}

	public void Display(float displayLength, string text)
	{
		StartCoroutine(DisplayFixedTime(displayLength, text));
	}

	IEnumerator DisplayFixedTime(float displayLength, string text)
	{
		SetText(text);

		while (displayLength > 0)
		{
			displayLength -= PlayTimer.Instance.TimePerFrame();

			yield return null;
		}

		gameObject.SetActive(false);
	}
}
