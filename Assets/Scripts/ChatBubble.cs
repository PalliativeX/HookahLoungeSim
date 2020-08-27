using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatBubble : MonoBehaviour
{
	private SpriteRenderer backgroundSpriteRenderer;
	private TextMeshPro textMeshPro;

	public static Transform prefab;

	public static void Create(Transform parent, Vector3 localPos, string text)
	{
		Transform chatBubbleTransform = Instantiate(prefab, parent);
		chatBubbleTransform.localPosition = localPos;

		chatBubbleTransform.GetComponent<ChatBubble>().SetUp(text);
	}

	private void Awake()
	{
		backgroundSpriteRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
		textMeshPro = transform.Find("Text").GetComponent<TextMeshPro>();
	}

	void Start()
	{
		SetUp("Hello world and enjoy bruh");
	}

	void SetUp(string text)
	{
		textMeshPro.SetText(text);
		textMeshPro.ForceMeshUpdate();
		Vector2 textSize = textMeshPro.GetRenderedValues(false);
		Vector2 padding = new Vector2(4f, 2f);
		backgroundSpriteRenderer.size = textSize + padding;
	}
}
