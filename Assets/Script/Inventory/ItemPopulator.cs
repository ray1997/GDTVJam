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

    public void OpenSubMenu()
    {
        if (InventoryScreen.Instance.CombiningMode)
        {
            InventoryScreen.CallRequestCombineItems(ItemInfo);
            return;
        }
        UseButton.Select();
        UseButtonMenu.SetActive(true);
        SendButton.interactable = ItemInfo.AllowSend && CanSend;
    }

    public void TryUseItem()
    {
        Debug.Log($"You try to use {ItemInfo.Name}");
        OnItemAttempUsage?.Invoke(ItemInfo);
        CloseUseMenu();
    }

    #region Context menu system (Leave to close menu)
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

    #endregion

    public Button SendButton;
    public bool CanSend;
    public void SendItem()
    {
        ElevatorControl.Instance.PutInItem(ItemInfo);
    }

    public GameObject CombinePrimaryActiveRing;
    public void CombineItem()
    {
        InventoryScreen.CallRequestCombineItemStart(ItemInfo);
        CombinePrimaryActiveRing.SetActive(true);
        CloseUseMenu();
        InventoryScreen.OnRequestCombineEnd += WaitForCombineModeEnd;
    }

    private void WaitForCombineModeEnd()
    {
        CombinePrimaryActiveRing.SetActive(false);
        InventoryScreen.OnRequestCombineEnd -= WaitForCombineModeEnd;
    }

    private void OnDestroy()
    {
        if (CombinePrimaryActiveRing.activeSelf)
            InventoryScreen.OnRequestCombineEnd -= WaitForCombineModeEnd;
    }
}