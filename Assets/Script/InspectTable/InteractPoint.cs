using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractPoint : MonoBehaviour
{
    public GameObject KeyItem;

    [SerializeField] bool _canInteract;
    public bool Interactable
    {
        get => _canInteract;
        set => _canInteract = value;
    }

    private void OnEnable()
    {
        PlayerInput.OnPlayerInteracted += Interacted;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerInteracted -= Interacted;
    }

    public void Interacted()
    {
        if (!Interactable)
            return;
        PlayerInteracted?.Invoke();
    }

    public UnityEvent PlayerInteracted;
}
