using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouselightControl : MonoBehaviour
{
    public Transform Player1;
    public Transform Player2;

    public float MaximumDistance;
    public Light[] Lights;
    
    private void Update()
    {
        if (!GameManager.Instance.GlobalElectricityStatus)
            return;
        foreach (var light in Lights)
        {
            if (Vector3.Distance(light.transform.position,
                PlayerSwitcher.Instance.CurrentPlayer == Player.First ? Player1.position : Player2.position) > MaximumDistance)
                light.enabled = false;
            else
                light.enabled = true;
        }
    }
}
