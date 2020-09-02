using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DirectionalLightManager : MonoBehaviour
{
    public float startIntensity = 1.1f;
    public float endIntensity = 0.1f;

    private Light dirLight;
    private Player player;

    WorkingHours workingHours;
    float totalHours;


    void Start()
    {
        player = FindObjectOfType<Player>();
        dirLight = GetComponent<Light>();
        workingHours = player.workingHours;
        totalHours = workingHours.ending - workingHours.beginning;
    }

    // TODO: Adjust this to simulate day-night cycle correctly,
    // now it's just an approximation based on working hours
    void Update()
    {
        DateTime time = PlayTimer.Instance.GetTime();
        float hoursMins = time.Hour + (time.Minute / 60f);
        if (hoursMins >= workingHours.beginning && hoursMins <= workingHours.ending)
        {
            float hoursDiff = hoursMins - workingHours.beginning + 0.01f;
            float delta = hoursDiff / totalHours;
            dirLight.intensity = Mathf.Lerp(startIntensity, endIntensity, delta);
        }
    }
}
