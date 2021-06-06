using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStatusUpdate : MonoBehaviour
{
    Light child;
    public MeshRenderer correspond_lampRender;
    private void Awake()
    {
        child = GetComponent<Light>();
        GameManager.ElectricityChanged += UpdateState;
    }

    private void Start()
    {
        child.enabled = GameManager.Instance.GlobalElectricityStatus;
    }

    private void UpdateState(bool status)
    {
        if (child != null)
            child.enabled = status;
        if (correspond_lampRender != null)
        {
            if (correspond_lampRender.materials.Length == 1) //One material lamp
                correspond_lampRender.material = status ?
                GameManager.Instance.LampOn : GameManager.Instance.LampOff;
            else if (correspond_lampRender.materials.Length == 2)
                correspond_lampRender.materials[1] = status ?
                    GameManager.Instance.DeskLampOn : GameManager.Instance.DeskLampOff;
        }
    }

    private void OnDestroy()
    {
        GameManager.ElectricityChanged -= UpdateState;
    }
}
