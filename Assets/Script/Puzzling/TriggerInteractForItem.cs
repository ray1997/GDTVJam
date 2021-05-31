using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerInteractForItem : MonoBehaviour
{
    public InGameItem ItemInfo;
    public Player Assigned;

    [SerializeField] bool _allowPickup;
    /// <summary>
    /// Just incase if player need to do other thing first
    /// </summary>
    public bool IsAllowToPickup
    {
        get => _allowPickup;
        set => _allowPickup = value;
    }

    //Enter area to interact
    [SerializeField] bool _canInteract;
    public bool Interactable
    {
        get => _canInteract;
        set => _canInteract = value;
    }
    public void OnTriggerEnter(Collider other)
    {
        Interactable = true;
    }

    public void OnTriggerExit(Collider other)
    {
        Interactable = false;
    }


    //Press E to pick up
    private void OnEnable()
    {
        PlayerInput.OnPlayerInteracted += Pickup;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerInteracted -= Pickup;
    }

    private void Pickup()
    {
        if (!IsAllowToPickup)
            return;
        if (!Interactable)
            return;
        PlayerState.RequestAddItem(ItemInfo, Assigned);
        Destroy(gameObject);
    }
}

public enum Specific
{
    None,
    UnlockFlashlight
}