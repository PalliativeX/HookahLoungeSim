using UnityEngine;
using System;

public class PointLightSwitcher : MonoBehaviour
{
    public Light pointLight;
    public WorkingHours workingHours = new WorkingHours(19, 8);

    DateTime currentTime;

	void Update()
    {
        currentTime = PlayTimer.Instance.GetTime();
        float hours = currentTime.Hour;
        if (hours >= workingHours.beginning || hours <= workingHours.ending)
        {
			if (!pointLight.enabled)
			{
				pointLight.enabled = true;
			}
		}
        else
        {
			if (pointLight.enabled)
			{
				pointLight.enabled = false;
			}
		}
    }


}
