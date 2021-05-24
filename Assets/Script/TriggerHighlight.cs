using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHighlight : MonoBehaviour
{
    public MeshRenderer[] TargetRenderers;

    public Material NormalMaterial;
    public Material HighlightMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var render in TargetRenderers)
            {
                render.material = HighlightMaterial;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (var render in TargetRenderers)
            {
                render.material = NormalMaterial;
            }
        }
    }
}
