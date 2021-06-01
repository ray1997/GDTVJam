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
        UpdateInventory();
    }

    public Image P1Portrait;
    public TMP_Text P1Location;

    public Image P2Portrait;
    public GameObject QuestBlueprint;
    private void UpdateInventory()
    {
        //Update player 1 infos
        //Portrait
        P1Portrait.sprite = Player1PortraitState[PlayerSwitch.CurrentPlayer == Player.First ? 1 : 0];
        //Location
        P1Location.text = Helper.help.TranslateColliderToLocationName(CameraSwitch.CurrentlyStayed1.name);
        //Objective
        foreach (var quest in Quest.ActiveObjectives)
        {
            if (quest.SubObjective.Length > 1)
            {
                if (quest.IsDone)
                    continue;
                if (quest.IsUnlock)
                {
                    //Put it on map?
                }
            }
            if (quest.IsDone)
                continue;
            if (quest.IsUnlock)
                break;
            //Put it on map!

        }


        P2Portrait.sprite = Player1PortraitState[PlayerSwitch.CurrentPlayer == Player.Second ? 1 : 0];
    }
}
