using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
        //Mark objective as complete
        Objectives.Instance.MarkQuestAsFinish(info.ItemInfo.ForQuestID);
        //Add item to inventory
        PlayerState.RequestAddItem(info.ItemInfo, info.Assigned);
        //Destroy pickup
        Destroy(info.gameObject);
    }

    public void PickupFlashlightPlayer2(TriggerInteractForItem info)
    {
        //Mark objective as complete
        Objectives.Instance.MarkQuestAsFinish(info.ItemInfo.ForQuestID);
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
        //Mark objective as complete
        Objectives.Instance.MarkQuestAsFinish(info.ItemInfo.ForQuestID);
        //Add item to inventory
        PlayerState.RequestAddItem(info.ItemInfo, info.Assigned);
        //Trigger post event
        PostP2PickupElevatorSwitch?.Invoke();
    }

    public UnityEvent PostP2PickupElevatorSwitch;

    public void PickupFuseP1(TriggerInteractForItem pick) 
    {
        Objectives.Instance.MarkQuestAsFinish(pick.ItemInfo.ForQuestID);

        PlayerState.RequestAddItem(pick.ItemInfo, pick.Assigned);

        PostP1PickupFuse?.Invoke();
    }
    public UnityEvent PostP1PickupFuse;

    public TMPro.TMP_Text ElectricityBoxFailStatus;
    public int FuseQuestID;
    public void InsertFuseToFusebox()
    {
        if (!Objectives.Instance.IsQuestUnlock(FuseQuestID))
            return;
        if (!Objectives.Instance.IsQuestFinish(FuseQuestID))
        {
            ShowNotificationOnText("You don't have a fuse yet.", ElectricityBoxFailStatus);
            return;
        }

        bool item = Player1Inventory.PlayerInventory.Any(i => i.ForQuestID == FuseQuestID);
        Debug.Log($"Fuse on player inventory status: {item}");
        if (item)
        {
            var fuse = Player1Inventory.PlayerInventory.First(i => i.ForQuestID == FuseQuestID);
            Player1Inventory.PlayerInventory.Remove(fuse);
            InventoryScreen.PrintLogText("Fuse reinstalled");
            FuseReinstalled?.Invoke();
        }
    }
    public UnityEvent FuseReinstalled;

    public void ShowNotificationOnText(string text, TMP_Text label)
    {
        if (label is null)
            return;
        label.text = text;
        label.DOFade(1, 0.5f);
        label.DOFade(0, 8f);
    }

    public void FlipOnElectricity()
    {
        GlobalElectricityStatus = true;
    }
}
