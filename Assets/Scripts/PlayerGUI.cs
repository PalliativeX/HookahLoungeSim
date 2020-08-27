using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
	public TMPro.TMP_Text ratingText, moneyText, timeText;
	public TMPro.TMP_Text statusSignText;

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
		timeText.text = player.playTimer.GetTimeStr();
	}

	public void UpdateRatingText()
	{
		ratingText.text = player.Rating.ToString();
	}

	public void UpdateStatusSignText()
	{
		statusSignText.text = (player.WorkStatus == WorkStatus.Open ? "OPEN" : "CLOSED");
	}
}
