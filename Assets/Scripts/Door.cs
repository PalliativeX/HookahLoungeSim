using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
	public Animator leftDoorAnimator;
	public Animator rightDoorAnimator;

	public bool currentlyOpen;

	public IEnumerator Open()
	{
		float timeToOpen = 2f;

		while (timeToOpen > 0f)
		{
			timeToOpen -= PlayTimer.Instance.TimePerFrame();
			yield return null;
		}

		ChangeState(open: true);
	}

	public void Close()
	{
		ChangeState(open: false);
	}

	private void ChangeState(bool open)
	{
		currentlyOpen = open;
		leftDoorAnimator.speed = PlayTimer.Instance.playSpeed;
		leftDoorAnimator.SetBool("Open", open);
		rightDoorAnimator.speed = PlayTimer.Instance.playSpeed;
		rightDoorAnimator.SetBool("Open", open);
	}
}
