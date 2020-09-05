using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameMenuManager gameMenuManager;

	public KeyCode pauseGameKey = KeyCode.Space;
	public KeyCode increaseGameSpeedKey = KeyCode.PageUp;
	public KeyCode decreaseGameSpeedKey = KeyCode.PageDown;

	bool paused;

	void Update()
	{
		if (Input.GetKeyDown(pauseGameKey))
		{
			paused = !paused;
			gameMenuManager.SwitchGamePause(paused);
		}

		if (Input.GetKeyDown(increaseGameSpeedKey))
		{
			gameMenuManager.ChangeGameSpeed(increase: true);
		}
		else if (Input.GetKeyDown(decreaseGameSpeedKey))
		{
			gameMenuManager.ChangeGameSpeed(increase: false);
		}
	}

}
