using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScreen : MonoBehaviour
{
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
    }

    private void OnDisable()
    {
        PlayerInput.OnRequestToggleInventory -= ToggleInventory;
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
        InventoryUI.SetActive(IsShowingInventory);
        if (IsShowingInventory)
            UpdateInventory();
    }

    public Image P1Portrait;
    public TMP_Text P1Location;

    public Image P2Portrait;
    public GameObject TaskTemplate;
    public GameObject ItemTemplate;
    private void UpdateInventory()
    {
        //Update player 1 infos
        //Portrait
        P1Portrait.sprite = Player1PortraitState[PlayerSwitch.CurrentPlayer == Player.First ? 1 : 0];
        P2Portrait.sprite = Player1PortraitState[PlayerSwitch.CurrentPlayer == Player.Second ? 1 : 0];
        //Location
        P1Location.text = Helper.help.TranslateColliderToLocationName(CameraSwitch.CurrentlyStayed1.name);
        //Objective
        foreach (var quest in Quest.ActiveObjectives)
        {
            if (quest.SubObjective.Length > 1)
            {
                foreach (var sub in quest.SubObjective)
                {
                    if (sub.IsDone)
                        continue;
                    if (!sub.IsUnlock)
                        continue;
                    PutATaskOntoMap(sub);
                }
                
            }
            if (quest.IsDone)
                continue;
            if (!quest.IsUnlock)
                continue;
            PutATaskOntoMap(quest);
        }
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
}
