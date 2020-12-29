using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingLights : MonoBehaviour
{
    float targetLight;
    float timerChange;
    public float lerp = 0.01f;
    Light light;
    void Start()
    {
        light = GetComponent<Light>();   
    }

    
    void Update()
    {
        light.intensity = Mathf.Lerp(light.intensity, targetLight, lerp);

        if (timerChange <= 0)
        {
            targetLight = Random.Range(0.1f, 1f);
            timerChange = Random.Range(1f, 4f);
        }
        timerChange -= Time.deltaTime;
    }
}
