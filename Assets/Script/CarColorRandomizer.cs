using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColorRandomizer : MonoBehaviour
{
    public List<Material> CarColors;
    public List<MeshRenderer> CarComp;

    public void OnEnable()
    {
        RandomizeColor();
    }

    private void RandomizeColor()
    {
        int index = UnityEngine.Random.Range(-2, CarColors.Count + 2);
        index = Mathf.Clamp(index, 0, CarColors.Count - 1);
        foreach (var car in CarComp)
        {
            car.material = CarColors[index];
        }
    }
}
