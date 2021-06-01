using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopulator : MonoBehaviour
{
    public delegate void UseItem(InGameItem info);
    public static event UseItem OnItemAttempUsage;

    public Image ItemIcon;
    public TMP_Text ItemName;
    public Button UseButton;
    public GameObject UseButtonMenu;
    public InGameItem ItemInfo;

    public void Initialize(InGameItem item)
    {
        ItemIcon.sprite = item.Icon;
        ItemName.text = item.Name;
        UseButtonMenu.SetActive(false);
        ItemInfo = item;
    }

    public void TryUseItem()
    {
        Debug.Log($"You try to use {ItemInfo.Name}");
        OnItemAttempUsage?.Invoke(ItemInfo);
        CloseUseMenu();
    }

    public void OpenUseMenu()
    {
        UseButtonMenu.SetActive(true);
        UseButton.Select();
    }


    public Selectable CurrentSelectionArea;
    public float DelayCloseTime = 0.25f;
    public void LeaveArea(Selectable item)
    {
        CurrentSelectionArea = null;
        Invoke(nameof(DelayCheck), DelayCloseTime);
    }

    public void EnterArea(Selectable item)
    {
        CurrentSelectionArea = item;
    }

    //Use by invoke this from certain delay, if nothing enter area assume player left
    public void DelayCheck()
    {
        if (CurrentSelectionArea is null)
            CloseUseMenu();
    }

    public void CloseUseMenu()
    {
        UseButtonMenu.SetActive(false);
    }
}
