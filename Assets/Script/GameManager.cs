using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public void PickupFlashlightPlayer1(TriggerInteractForItem info)
    {
        if (!info.Interactable) { Debug.Log("Can't pickup flashlight. It's not within interaction range"); return; }
        if (!info.IsAllowToPickup) { Debug.LogError("Not allow to pickup, previous quest not complete"); return; }
        //Mark objective as complete
        Objectives.RequestFinishQuest(info.ItemInfo.ForQuestID, info.Assigned);
        //Add item to inventory
        PlayerState.RequestAddItem(info.ItemInfo, info.Assigned);
        //Destroy pickup
        Destroy(info.gameObject);
    }

    public void PickupFlashlightPlayer2(TriggerInteractForItem info)
    {
        if (!info.Interactable) { Debug.Log("Can't pickup flashlight. It's not within interaction range"); return; }
        if (!info.IsAllowToPickup) { Debug.LogError("Not allow to pickup, previous quest not complete"); return; }
        //Mark objective as complete
        Objectives.RequestFinishQuest(info.ItemInfo.ForQuestID, info.Assigned);
        //Add item to inventory
        PlayerState.RequestAddItem(info.ItemInfo, info.Assigned);
        //Trigger post event
        PostP2PickupFlashlight?.Invoke();
    }

    //Destroy torch item
    //Destroy pickup
    public UnityEvent PostP2PickupFlashlight;

    public void PickupFoodElevatorSwitch(TriggerInteractForItem info)
    {
        if (!info.Interactable) { Debug.Log("Can't pickup switch. It's not within interaction range"); return; }
        if (!info.IsAllowToPickup) { Debug.LogError("Not allow to pickup, previous quest not complete"); return; }
        //Mark objective as complete
        Objectives.RequestFinishQuest(info.ItemInfo.ForQuestID, info.Assigned);
        //Add item to inventory
        PlayerState.RequestAddItem(info.ItemInfo, info.Assigned);
        //Trigger post event
        PostP2PickupElevatorSwitch?.Invoke();
    }

    public UnityEvent PostP2PickupElevatorSwitch;

    public void PickupPowerFuse(TriggerInteractForItem info)
    {
        if (!info.Interactable) { Debug.Log("Can't pickup switch. It's not within interaction range"); return; }
        if (!info.IsAllowToPickup) { Debug.LogError("Not allow to pickup, previous quest not complete"); return; }
        //Mark objective as complete
        Objectives.RequestFinishQuest(info.ItemInfo.ForQuestID, info.Assigned);
        //Add item to inventory
        PlayerState.RequestAddItem(info.ItemInfo, info.Assigned);
        //Trigger post event
        PostPickupPowerFuse?.Invoke();
    }
    public UnityEvent PostPickupPowerFuse;
}
