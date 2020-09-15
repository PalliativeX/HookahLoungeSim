using System.Collections;
using UnityEngine;

public class Hookah : MonoBehaviour
{
	public Tobacco ContainedTobacco;

	public bool Occupied { get; set; }
	public bool Active { get; set; }
	public Client Client { get; set; }

	float maxSmokingTime;
	float smokingTimeLeft;

	public void SetActive()
	{
		float smokingTime = ContainedTobacco.brand.smokingTime;
		smokingTimeLeft = smokingTime;
		Active = true;

		StartCoroutine(Smoke());
	}

	IEnumerator Smoke()
	{
		while (smokingTimeLeft > 0f)
		{
			smokingTimeLeft -= PlayTimer.Instance.TimePerFrame();
			yield return null;
		}

		Active = false;
		smokingTimeLeft = 0f;
	}
}
