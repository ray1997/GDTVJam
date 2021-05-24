using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectableTrigger : MonoBehaviour
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
                    PlayerInput.OnPlayerInteracted += EnterInspectView;
                else
                    PlayerInput.OnPlayerInteracted -= LeaveInspectView;
            }
        }
    }

    private void LeaveInspectView()
    {
        Inspector.LeaveTable();
    }

    private void EnterInspectView()
    {
        Inspector.InspectTable();
    }
    public Inspect Inspector;

    private void OnTriggerEnter(Collider collide)
    {
        if (collide.tag == "Player")
        {
            CanInteract = true;
        }
    }

    private void OnTriggerExit(Collider collide)
    {
        if (collide.tag == "Player")
        {
            CanInteract = false;
        }
    }
}
