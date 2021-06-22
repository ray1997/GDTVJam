using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPopulator : MonoBehaviour
{
    public delegate void UseItem(InGameItem info);
    public static event UseItem OnItemAttempUsage;
    public Player Possessor;

    public Image ItemIcon;
    public TMP_Text ItemName;
    public GameObject UseButtonMenu;
    public Button UseButton;
    public Button MixButton;
    public Button CombineButton;
    public Button SendButton;
    public Button GiveButton;
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
            ContextMenu.UseMenuLayout.CalculateLayoutInputVertical();
            if (Possessor == Player.First)
                GiveButton.gameObject.SetActive(ApproximityCheck.P2Instance.WithinRange);
            else if (Possessor == Player.Second)
                GiveButton.gameObject.SetActive(ApproximityCheck.P1Instance.WithinRange);
            StartCoroutine(RefreshMenuLayout());
        }
    }

    IEnumerator RefreshMenuLayout()
    {
        ContextMenu.UseMenuLayout.enabled = false;
        yield return new WaitForEndOfFrame();
        ContextMenu.UseMenuLayout.enabled = true;
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

    public void GiveItem()
    {
        Debug.Log($"Giving item {ItemInfo.Name} from {Possessor} to {OpposePossessor(Possessor)}");
        var item = ItemInfo;
        if (Possessor == Player.First && ApproximityCheck.P2Instance.WithinRange)
        {
            //Take item from first player and put it on 2nd player inv
            GameManager.Instance.Player1Inventory.PlayerInventory.Remove(item);
            GameManager.Instance.Player2Inventory.PlayerInventory.Add(item);
        }
        else if (Possessor == Player.Second && ApproximityCheck.P1Instance.WithinRange)
        {
            GameManager.Instance.Player2Inventory.PlayerInventory.Remove(item);
            GameManager.Instance.Player1Inventory.PlayerInventory.Add(item);
        }
        PlayerState.UpdateItemTransferStatus(item, Possessor, OpposePossessor(Possessor));
        InventoryScreen.ForceUpdateInventory();
        //if (Possessor == Player.First && !ApproximityCheck.P2Instance.WithinRange)
        //    return;
        //if (Possessor == Player.Second && !ApproximityCheck.P1Instance.WithinRange)
        //    return;
        //InventoryUpdateRequirementArgs remove = new InventoryUpdateRequirementArgs(Possessor,
        //    ItemInfo,
        //    UpdateType.ItemRemoved,
        //    true, false);
        //InventoryUpdateRequirementArgs add = new InventoryUpdateRequirementArgs(OpposePossessor(Possessor),
        //    ItemInfo,
        //    UpdateType.ItemAdded,
        //    false, false);
        //PlayerState.ForcefullyManipulateInventory(add);
        //PlayerState.ForcefullyManipulateInventory(remove);
        //InventoryScreen.Instance.UpdateInventoryItems();
    }

    public Player OpposePossessor(Player possessor) =>
        possessor == Player.First ? Player.Second : Player.First;
}