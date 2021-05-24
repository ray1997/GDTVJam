using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerHighlight : MonoBehaviour
{
    public MeshRenderer[] TargetRenderers;

    List<Material> NormalMaterials;
    public Material HighlightMaterial;

    private void Awake()
    {
        NormalMaterials = new List<Material>();
        foreach (var render in TargetRenderers)
        {
            NormalMaterials.Add(render.material);
        }
    }

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
            RestoreMaterials();
        }
    }

    public void RestoreMaterials()
    {
        for (int i = 0; i < TargetRenderers.Length; i++)
        {
            TargetRenderers[i].material = NormalMaterials[i];
        }
    }
}
