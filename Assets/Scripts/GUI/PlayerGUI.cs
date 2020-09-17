using UnityEngine;
using TMPro;

public class PlayerGUI : MonoBehaviour
{
	const int maxRatingLength = 3;

	public TransparentObjectsController transparentObjectsController;

	public TMP_Text ratingText, moneyText, timeText;
	public TMP_Text statusSignText;

	Player player;

	void Start()
    {
		player = GetComponent<Player>();
		UpdateMoneyText();
		UpdateStatusSignText();
		UpdateRatingText();
	}

    void Update()
    {
		UpdateTimeText();
	}

	public void UpdateMoneyText()
	{
		moneyText.text = player.Money.ToString() + "$";
	}

	public void UpdateTimeText()
	{
		timeText.text = PlayTimer.Instance.GetTimeStr();
	}

	public void UpdateRatingText()
	{
		string ratingTextStr = player.Rating.ToString();
		if (ratingTextStr.Length > maxRatingLength)
		{
			ratingTextStr = ratingTextStr.Substring(0, maxRatingLength);
		}	
		ratingText.text = ratingTextStr;
	}

	public void UpdateStatusSignText()
	{
		statusSignText.text = (player.WorkStatus == WorkStatus.Open ? "OPEN" : "CLOSED");
	}

}
