using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamerLamp : MonoBehaviour
{
    public Material[] Swaps;
    public MeshRenderer Target;

    public Gradient LampColors;
    public float SpeedModified = 1;
    Light lamp;
    private void Awake()
    {
        lamp = GetComponent<Light>();
    }

    public float TimeCounter;
    public bool Pingpong;
    void Update()
    {
        if (Pingpong)
            TimeCounter += Time.deltaTime * SpeedModified;
        else
            TimeCounter -= Time.deltaTime * SpeedModified;
        if (TimeCounter > 1)
        {
            TimeCounter = 1;
            Pingpong = !Pingpong;
        }
        else if (TimeCounter < 0)
        {
            TimeCounter = 0;
            Pingpong = !Pingpong;
        }

        if (Target != null)
        {
            //Different mode
            lamp.intensity = Mathf.Lerp(0.2f, 0, TimeCounter);
            Target.material = Swaps[Pingpong ? 0 : 1];
        }
        else
        {
            lamp.color = LampColors.Evaluate(TimeCounter);
        }
    }
}
