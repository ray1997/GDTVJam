using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightControl : MonoBehaviour
{
    bool _canInteract;
    public bool CanInteract
    {
        get => _canInteract;
        set
        {
            if (!Equals(_canInteract, value))
            {
                if (value)
                    PlayerInput.OnPlayerInteracted += AllowLightControl;
                else
                    PlayerInput.OnPlayerInteracted -= AllowLightControl;
                _canInteract = value;
            }
        }
    }

    public GameObject ControlledLight;
    public Transform VisualSwitch;
    private void AllowLightControl()
    {
        //Toggle light visibility
        ControlledLight.SetActive(!ControlledLight.activeSelf);
        //Rotate visual switch
        if (ControlledLight.activeSelf)
            VisualSwitch.Rotate(new Vector3(22, 0, 0), Space.World);
        else
            VisualSwitch.Rotate(new Vector3(-22, 0, 0), Space.World);
    }

    private void OnTriggerEnter(Collider collide)
    {
        Debug.Log($"Player enter {gameObject.name} collider");
        if (collide.tag == "Player")
        {
            CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider collide)
    {
        Debug.Log($"Player leave {gameObject.name} collider");
        if (collide.tag == "Player")
        {
            CanInteract = false;
        }
    }
}
