using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InventoryScreen : MonoBehaviour
{
    public static InventoryScreen Instance;
    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public delegate void RequestUpdateInventory();
    public static event RequestUpdateInventory TakingRequestUpdateInventory;
    public static void ForceUpdateInventory() => TakingRequestUpdateInventory?.Invoke();

    public delegate void UpdateItemUsageLog(string message);
    public static event UpdateItemUsageLog OnLogAdded;
    public static void PrintLogText(string text) => OnLogAdded?.Invoke(text);

    public GameObject InventoryUI;

    public PlayerState Player1;
    public PlayerState Player2;
    public PlayerSwitcher PlayerSwitch;
    public BetterCameraSwitcher CameraSwitch;
    public Objectives Quest;

    //0 for inactive
    public Sprite[] Player1PortraitState;
    public Sprite[] Player2PortraitState;

    private void OnEnable()
    {
        PlayerInput.OnRequestToggleInventory += ToggleInventory;
        OnRequestCombineStart += StartItemCombineMode;
        OnRequestCombineEnd += EndItemCombineMode;
    }

    public TMP_Text LogDisplay;
    private void UpdateLogText(string message)
    {
        LogDisplay.text = message;
    }

    private void OnDisable()
    {
        PlayerInput.OnRequestToggleInventory -= ToggleInventory;
        OnRequestCombineStart -= StartItemCombineMode;
        OnRequestCombineEnd -= EndItemCombineMode;
    }

    bool _show;
    public bool IsShowingInventory
    {
        get => _show;
        set => _show = value;
    }
    private void ToggleInventory()
    {
        IsShowingInventory = !IsShowingInventory;
        //Move UI out of screen
        if (IsShowingInventory)
            InventoryUI.transform.DOLocalMove(Vector3.zero, 0.5f);
        else
            InventoryUI.transform.DOLocalMove(new Vector3(0, (Screen.height * 5) * - 1), 0.5f);
        //
        if (IsShowingInventory)
        {
            UpdateInventory();
            TakingRequestUpdateInventory += UpdateInventoryItems;
            OnLogAdded += UpdateLogText;
        }
        else
        {
            TakingRequestUpdateInventory -= UpdateInventoryItems;
            OnLogAdded -= UpdateLogText;
        }
    }

    public Image P1Portrait;
    public TMP_Text P1Location;

    public Image P2Portrait;
    public TMP_Text P2Location;
    public GameObject TaskTemplate;
    public GameObject ItemTemplate;
    private void UpdateInventory()
    {
        //Update player 1 infos
        //Portrait
        P1Portrait.sprite = Player1PortraitState[PlayerSwitch.CurrentPlayer == Player.First ? 1 : 0];
        P2Portrait.sprite = Player1PortraitState[PlayerSwitch.CurrentPlayer == Player.Second ? 1 : 0];
        //Location
        P1Location.text = Helper.help.TranslateColliderToLocationName(CameraSwitch.CurrentlyStayed1);
        P2Location.text = Helper.help.TranslateColliderToLocationName(CameraSwitch.CurrentlyStayed2);
        //Objective
        foreach (var quest in Quest.ActiveObjectives)
        {
            if (quest.IsDone)
                continue;
            if (!quest.IsUnlock)
                continue;
            PutATaskOntoMap(quest);
        }
        UpdateInventoryItems();
    }

    public void UpdateInventoryItems()
    {
        //Items
        //Player 1
        //Clear previous
        Helper.help.ChildObliterator(Player1Inventory);
        foreach (var item in Player1.PlayerInventory)
        {
            GameObject child = Instantiate(ItemTemplate, Player1Inventory, false);
            child.name = item.Name;
            child.SetActive(true);
            ItemPopulator info = child.GetComponent<ItemPopulator>();
            info.CanSend = ElevatorControl.Instance.OverallCanUse
                && ElevatorControl.Instance.WithinActiveZone
                && ElevatorControl.Instance.ActivePlayer == ElevatorPlayer.Player1;
            info.Initialize(item);
        }
        //Player 2
        //Clear previous
        Helper.help.ChildObliterator(Player2Inventory);
        foreach (var item in Player2.PlayerInventory)
        {
            GameObject child = Instantiate(ItemTemplate, Player2Inventory, false);
            child.name = item.Name;
            child.SetActive(true);
            ItemPopulator info = child.GetComponent<ItemPopulator>();
            info.CanSend = ElevatorControl.Instance.OverallCanUse
                 && ElevatorControl.Instance.WithinActiveZone
                 && ElevatorControl.Instance.ActivePlayer == ElevatorPlayer.Player2;
            info.Initialize(item);
        }
    }

    public Transform Player1Inventory;
    public Transform Player2Inventory;

    public GameObject FirstFloorUIPoints;
    public GameObject SecondFloorUIPoints;
    public void PutATaskOntoMap(ObjectiveInfo info)
    {
        //Put it on map!
        string location = info.Location.ToString();
        Transform locateParent = FirstFloorUIPoints.transform.Find(location);
        if (locateParent is null)
            locateParent = SecondFloorUIPoints.transform.Find(location);
        if (locateParent is null)
            return;
        //Check if this task already exist or not
        Transform prevChild = locateParent.Find(info.ID.ToString());
        if (prevChild is null) //Previous child isn't exist, make a new child. OwO
        {
            var child = Instantiate(TaskTemplate, locateParent, false);
            child.SetActive(true);
            child.name = info.ID.ToString();
            var task = child.GetComponent<TaskPopulator>();
            task.Initialize(info);
        }
    }

    public InGameItem PrimaryCombineItem;
    public bool CombiningMode;
    
    public delegate void RequestCombineBegin(InGameItem primary);
    public static event RequestCombineBegin OnRequestCombineStart;
    public static void CallRequestCombineItemStart(InGameItem item) => OnRequestCombineStart?.Invoke(item);

    public delegate void RequestCombineItems(InGameItem itemA, InGameItem itemB);
    public static event RequestCombineItems OnRequestCombineItems;
    public static void CallRequestCombineItems(InGameItem item)
    {
        if (item == Instance.PrimaryCombineItem)
        {
            OnRequestCombineEnd?.Invoke();
            return;
        }
        OnRequestCombineItems?.Invoke(Instance.PrimaryCombineItem, item);
    } 

    public delegate void RequestCombineDone();
    public static event RequestCombineDone OnRequestCombineEnd;
    public static void CallRequestCombineItemEnd() => OnRequestCombineEnd?.Invoke();

    public void StartItemCombineMode(InGameItem item)
    {
        PrimaryCombineItem = item;
        CombiningMode = true;
    }

    public void EndItemCombineMode()
    {
        PrimaryCombineItem = null;
        CombiningMode = false;        
    }
}
