using UnityEngine;

public class CameraController : MonoBehaviour
{
	public KeyCode switchToKitchen = KeyCode.UpArrow;
	public KeyCode switchToLounge = KeyCode.DownArrow;

	public Location CurrentLocation { get; set; }

	public Vector3 cameraAdvance = new Vector3(0, 0, 48);

	void Start()
    {
		CurrentLocation = Location.Lounge;
    }

    void Update()
    {
		if (Input.GetKeyDown(switchToKitchen) && CurrentLocation != Location.Kitchen)
		{
			CurrentLocation = Location.Kitchen;
			transform.position += cameraAdvance;
		}
		else if (Input.GetKeyDown(switchToLounge) && CurrentLocation != Location.Lounge)
		{
			CurrentLocation = Location.Lounge;
			transform.position -= cameraAdvance;
		}
	}


}
