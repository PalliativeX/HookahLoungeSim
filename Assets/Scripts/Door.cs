using UnityEngine;

public class Door : MonoBehaviour
{
	public Animator leftDoorAnimator;
	public Animator rightDoorAnimator;

	public bool currentlyOpen;

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			currentlyOpen = !currentlyOpen;
			leftDoorAnimator.SetBool("Open", currentlyOpen);
			rightDoorAnimator.SetBool("Open", currentlyOpen);
		}
	}
}
