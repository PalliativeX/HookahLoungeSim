using UnityEngine;

public class Door : MonoBehaviour
{
	public Animator leftDoorAnimator;
	public Animator rightDoorAnimator;

	public bool currentlyOpen;

	public void Open()
	{
		ChangeState(open: true);
	}

	public void Close()
	{
		ChangeState(open: false);
	}

	private void ChangeState(bool open)
	{
		currentlyOpen = open;
		leftDoorAnimator.SetBool("Open", open);
		rightDoorAnimator.SetBool("Open", open);
	}
}
