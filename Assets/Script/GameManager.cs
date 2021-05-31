using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Material LampOn;
    public Material LampOff;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    bool _havePower;
    public bool GlobalElectricityStatus
    {
        get => _havePower;
        set
        {
            if (!Equals(_havePower, value))
            {
                ElectricityChanged?.Invoke(value);
            }
            _havePower = value;
        }
    }

    public delegate void ElectricityStatusUpdate(bool status);
    public static event ElectricityStatusUpdate ElectricityChanged;

    public PlayerState Player1Inventory;
    public PlayerState Player2Inventory;

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

    public TMPro.TMP_Text ElectricityBoxFailStatus;
    public int FuseQuestID;
    public void InsertFuseToFusebox()
    {
        if (!Objectives.Instance.IsQuestUnlock(FuseQuestID))
            return;
        if (!Objectives.Instance.IsQuestFinish(FuseQuestID))
        {
            ElectricityBoxFailStatus.text = "You don't have a fuse yet.";
            return;
        }

        bool item = Player1Inventory.PlayerInventory.Any(i => i.ForQuestID == FuseQuestID);
        Debug.Log($"Fuse on player inventory status: {item}");
        if (item)
        {
            var fuse = Player1Inventory.PlayerInventory.First(i => i.ForQuestID == FuseQuestID);
            Player1Inventory.PlayerInventory.Remove(fuse);
            ElectricityBoxFailStatus.text = "Fuse reinstalled";
            FuseReinstalled?.Invoke();
        }
    }
    public UnityEvent FuseReinstalled;

    public void FlipOnElectricity()
    {
        GlobalElectricityStatus = true;
    }
}
