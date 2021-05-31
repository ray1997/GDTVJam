using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerInteractForItem : MonoBehaviour
{
    public InGameItem ItemInfo;
    public Player Assigned;

    public int WaitedQuestID;
    private void WaitForUnlockFromQuest(ObjectiveInfo sender, int id)
    {
        Debug.Log($"[{name}] Request unlock from quest ID {sender.ID} ({sender.Name})" +
            $"Item pickup {(WaitedQuestID == id ? "allowed" : "disallowed")}" +
            $"Wanted quest ID: {WaitedQuestID}");
        if (WaitedQuestID == id)
        {
            IsAllowToPickup = true;
        }
    }

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
        if (!RequireClick)
            PlayerInput.OnPlayerInteracted += Pickup;
    }

    private void OnDisable()
    {
        if (!RequireClick)
            PlayerInput.OnPlayerInteracted -= Pickup;
    }

    private void Awake()
    {
        if (WaitedQuestID != 0)
            ObjectiveInfo.OnObjectiveFinished += WaitForUnlockFromQuest;
    }

    public bool RequireClick;
    public void Pickup()
    {
        if (!IsAllowToPickup)
            return;
        if (!Interactable)
            return;
        Debug.Log($"Picked up {ItemInfo?.Name}");
        if (!(ItemInfo is null))
            PlayerState.RequestAddItem(ItemInfo, Assigned);
        ObjectiveInfo.OnObjectiveFinished -= WaitForUnlockFromQuest;
        if (!RequireClick)
            PlayerInput.OnPlayerInteracted -= Pickup;
        PostPickup?.Invoke();
    }

    public UnityEvent PostPickup;
}

public enum Specific
{
    None,
    UnlockFlashlight
}