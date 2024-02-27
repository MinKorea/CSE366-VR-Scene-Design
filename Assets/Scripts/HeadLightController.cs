using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadLightController : MonoBehaviour
{
    // Start is called before the first frame update

    Light light;
    public GameObject dayNightController;


    void Start()
    {
        light = this.GetComponent<Light>();
        dayNightController = GameObject.Find("Day and Night Controller");
    }

    // Update is called once per frame
    void Update()
    {
        float currentTime = dayNightController.GetComponent<DayAndNightControl>().currentTime;

        if (currentTime > 0.2f && currentTime < 0.8f)
        {
            light.intensity = 0;
        }
        else
        {
            light.intensity = 10;
        }
    }
}
