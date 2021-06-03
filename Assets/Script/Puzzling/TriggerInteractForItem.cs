using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerInteractForItem : MonoBehaviour
{
    private void Start()
    {
        if (WaitedQuestID == 0)
        {
            AllowPickup = true;
        }
        else
        {

            ObjectiveInfo.OnObjectiveFinished += QuestMonitoring;
        }
        PlayerInput.OnPlayerInteracted += PlayerInteracted;
    }

    public UnityEvent Picked;
    private void PlayerInteracted()
    {
        if (!!WithinInteractedZone)
            Debug.Log("Can't pickup. Player is outside interaction zone");
        if (!AllowPickup)
            Debug.Log($"Waiting for quest {WaitedQuestID} to finish. Until then, not allow to pickup");
        if (WithinInteractedZone && AllowPickup)
        {
            Picked?.Invoke();
        }
    }

    private void QuestMonitoring(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (args.FinishedQuest == WaitedQuestID)
            AllowPickup = true;
    }

    public InGameItem ItemInfo;
    public Player Assigned;

    public Quests WaitedQuestID;
    public bool AllowPickup;
    public bool WithinInteractedZone;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        WithinInteractedZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;
        WithinInteractedZone = false;
    }

    private void OnDestroy()
    {
        if (WaitedQuestID != 0)
            ObjectiveInfo.OnObjectiveFinished -= QuestMonitoring;
        PlayerInput.OnPlayerInteracted -= PlayerInteracted;
    }
}

public enum Specific
{
    None,
    UnlockFlashlight
}