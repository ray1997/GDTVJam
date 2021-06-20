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
    public GameObject UseButtonMenu;
    public Button UseButton;
    public Button MixButton;
    public Button CombineButton;
    public Button SendButton;
    public InGameItem ItemInfo;

    public GameObject MixingButtonMenu;

    public ContextMenuSystem ContextMenu;

    public void Initialize(InGameItem item)
    {
        ItemIcon.sprite = item.Icon;
        ItemName.text = item.Name;
        UseButtonMenu.SetActive(false);
        ItemInfo = item;
    }

    public void OpenSubMenu()
    {
        if (InventoryScreen.Instance.CurrentItemMode == ItemMode.Combine)
        {
            InventoryScreen.CallRequestCombineItems(ItemInfo);
            return;
        }
        else if (InventoryScreen.Instance.CurrentItemMode == ItemMode.Mix)
        {
            //Open mix menu
            MixingButtonMenu.SetActive(true);
            return;
        }
        else if (InventoryScreen.Instance.CurrentItemMode == ItemMode.None)
        {
            //Open use menu
            UseButtonMenu.SetActive(true);
            UseButton.Select();
            CombineButton.gameObject.SetActive(ItemInfo.AllowCombine);
            MixButton.gameObject.SetActive(ItemInfo.AllowMix);
            SendButton.gameObject.SetActive(ItemInfo.AllowSend && CanSend);
        }
    }

    public void TryUseItem()
    {
        Debug.Log($"You try to use {ItemInfo.Name}");
        OnItemAttempUsage?.Invoke(ItemInfo);
        ContextMenu.ForceClose();
    }

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
        ContextMenu.ForceClose();
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
        {
            InventoryScreen.OnRequestCombineEnd -= WaitForCombineModeEnd;
            InventoryScreen.OnRequestMixingEnd -= WaitForMixModeEnd;
        }
    }

    public void MixItem()
    {
        //Force close current menu
        ContextMenu.ForceClose();
        //Start mix mode
        InventoryScreen.CallRequestMixingItemStart(ItemInfo);
        //Show selection ring
        CombinePrimaryActiveRing.SetActive(true);
        //Register for when it's gonna end.
        InventoryScreen.OnRequestMixingEnd += WaitForMixModeEnd;
    }

    public void AddIngredient()
    {
        //Force close context menu
        ContextMenu.ForceClose();
        //Add item event
        InventoryScreen.CallRequestMixingItemAdd(ItemInfo);
        //Show ring
        CombinePrimaryActiveRing.SetActive(true);
        //Wait for its end
        InventoryScreen.OnRequestMixingEnd += WaitForMixModeEnd;
    }

    public void FinalizeMix()
    {
        //Force close context menu
        ContextMenu.ForceClose();
        //Add item event
        InventoryScreen.CallRequestMixingItemEnd(ItemInfo);
    }

    private void WaitForMixModeEnd(InGameItem item)
    {
        CombinePrimaryActiveRing.SetActive(false);
    }
}