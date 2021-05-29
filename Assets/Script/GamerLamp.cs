using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamerLamp : MonoBehaviour
{
    public Gradient LampColors;
    public float SpeedModified = 1;
    Light lamp;
    private void Awake()
    {
        lamp = GetComponent<Light>();
    }

    public float TimeCounter;
    void Update()
    {
        TimeCounter += Time.deltaTime * SpeedModified;
        if (TimeCounter > 1)
            TimeCounter = 0;
        lamp.color = LampColors.Evaluate(TimeCounter);
    }
}
