using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ElevatorControl : MonoBehaviour
{
    #region Singleton stuff
    public static ElevatorControl Instance;
    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void LeftArea(Collider other, string name)
    {
        Debug.Log($"[{name}] {other.name} left the area");
        if (!other.CompareTag("Player"))
        {
            Debug.LogWarning("Not player! Ignore!");
            return;
        }
        ActivePlayer = ElevatorPlayer.None;
        ActiveFloor = Floor.None;
        WithinActiveZone = false;
        //TODO:Close door
    }

    public void FocusTo(Collider other, string name)
    {
        if (!other.CompareTag("Player"))
            return;
        ActivePlayer = (ElevatorPlayer)Enum.Parse(typeof(ElevatorPlayer), other.name);
        ActiveFloor = name.EndsWith("1") ? Floor.First : Floor.Second;
        WithinActiveZone = OverallCanUse;
    }
    #endregion

    public ElevatorPlayer ActivePlayer = ElevatorPlayer.None;
    public Floor ActiveFloor;
    public bool WithinActiveZone;

    #region Elevator status
    public GameObject SwitchVisualDisplay;
    [SerializeField] bool _switchVisible;
    public bool HasSwitch => SwitchVisualDisplay.activeSelf;
    public bool IsActivated => Objectives.Instance.IsQuestUnlock((int)Quests.P2ActivateElevator) &&
                Objectives.Instance.IsQuestFinish((int)Quests.P2ActivateElevator);
    public bool HasElectricity => GameManager.Instance.GlobalElectricityStatus;

    public bool OverallCanUse
    {
        get
        {
            //Status check
            if (!HasElectricity) //Is electricity online?
                return false;
            if (!HasSwitch) //Switch is visible on second floor?
                return false;
            if (!IsActivated) //Elevator activated?
                return false;
            return true;
        }
    }

    public bool IsOnRightFloor
    {
        get
        {
            //Is elevator inside the same floor as where it activated
            if (ActiveFloor == Floor.None)
                return false;
            if (ActiveFloor == Floor.First)
            {
                if (ElevatorInside.localPosition.y != 0) //Is elevator shaft at first floor?
                    return false;
                if (ElevatorDoorF1.localPosition.y == 0) //Is elevator door open?
                    return false;
            }
            else if (ActiveFloor == Floor.Second)
            {
                if (ElevatorInside.localPosition.y != ElevatorFloor2PosY) //Is elevator shaft at second floor?
                    return false;
                if (ElevatorDoorF2.localPosition.y == 0) //Is elevator door open?
                    return false;
            }
            return true;
        }
    }
    #endregion

    public Transform ElevatorDoorF1;
    public Transform ElevatorDoorF2;

    public Transform ElevatorInside;

    public float ElevatorTravelTime = 5f;
    public float ElevatorFloor2PosY = 5.05f;
    public void CallToFirstFloor()
    {
        var seq = DOTween.Sequence().
            Append(ElevatorDoorF2.DOLocalMoveY(0, 1)).
            Append(ElevatorInside.DOLocalMoveY(0, ElevatorTravelTime)).
            Append(ElevatorDoorF1.DOLocalMoveY(-0.6f, 1));
        seq.OnStart(() => { ElevatorGoingDownward?.Invoke(); });
        seq.OnComplete(() => { ElevatorReachDestination?.Invoke(); });
        seq.Play();
    }

    public void CallToSecondFloor()
    {
        var seq = DOTween.Sequence().
            Append(ElevatorDoorF1.DOLocalMoveY(0, 1)).
            Append(ElevatorInside.DOLocalMoveY(ElevatorFloor2PosY, ElevatorTravelTime)).
            Append(ElevatorDoorF2.DOLocalMoveY(-0.6f, 1));
        seq.OnStart(() => { ElevatorGoingUpward?.Invoke(); });
        seq.OnComplete(() => { ElevatorReachDestination?.Invoke(); });
        seq.Play();
    }

    private void OnEnable()
    {
        PlayerInput.OnPlayerInteracted += CallElevator;
    }

    private void CallElevator()
    {
        Debug.Log("Call elevator via key");
        if (!IsOnRightFloor)
        {
            if (ActiveFloor == Floor.First)
                CallToFirstFloor();
            else if (ActiveFloor == Floor.Second)
                CallToSecondFloor();
            return;
        }
        if (HoldingItem != null)
            YoinkItem();
    }

    //#region Item holding
    private void YoinkItem()
    {
        //Add to inventory
        PlayerState.RequestAddItem(HoldingItem, PlayerSwitcher.Instance.CurrentPlayer);
        OnElevatorItemChanged?.Invoke(this, new ElevatorItemUpdatedArgs(HoldingItem, UpdateType.ItemRemoved));
        //Remove model
        Destroy(HoldingModel);
        HoldingModel = null;
        //Reset holding item info
        HoldingItem = null;
    }
    public Transform ElevatorInsideParent;
    public InGameItem HoldingItem;
    public GameObject HoldingModel;

    public void PutInItem(InGameItem item)
    {
        if (!item.AllowSend)
            return;
        //Take it off from inventory
        PlayerState.RequestRemoveItem(item, PlayerSwitcher.Instance.CurrentPlayer);
        //Show model in elevator shaft
        var model = Instantiate(item.Model, ElevatorInsideParent, false);
        HoldingModel = model;
        //Set item holding info
        HoldingItem = item;
        OnElevatorItemChanged?.Invoke(this, new ElevatorItemUpdatedArgs(item, UpdateType.ItemAdded));
    }

    public delegate void ElevatorItemUpdate(ElevatorControl sender, ElevatorItemUpdatedArgs args);
    public static event ElevatorItemUpdate OnElevatorItemChanged;

    public delegate void ElevatorGoUp();
    public static event ElevatorGoUp ElevatorGoingUpward;

    public delegate void ElevatorGoDown();
    public static event ElevatorGoDown ElevatorGoingDownward;

    public delegate void ElevatorStopped();
    public static event ElevatorStopped ElevatorReachDestination;

    public Material[] RedLight;
    public Material[] GreenLight;

    public Floor CurrentFloor
    {
        get
        {
            if (ElevatorInside.localPosition.y == 0)
                return Floor.First;
            else if (ElevatorInside.localPosition.y == ElevatorFloor2PosY)
                return Floor.Second;
            return Floor.None;
        }
    }
}

public class ElevatorItemUpdatedArgs : System.EventArgs
{
    public UpdateType TriggerStatus { get; private set; }
    public InGameItem TriggerItem { get; private set; }
    public ElevatorItemUpdatedArgs(InGameItem item, UpdateType update)
    {
        TriggerStatus = update;
        TriggerItem = item;
    }
}
public enum UpdateType
{
    ItemAdded,
    ItemRemoved
}
public enum Floor
{
    First,
    Second,
    None
}

public enum ElevatorPlayer
{
    Player1,
    Player2,
    None
}