using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public List<InGameItem> PlayerInventory;

    Player InventoryOfPlayer;
    Player CurrentlyActive;
    public GameObject Flashlight;
    [SerializeField] bool _haveFlashlight;
    public bool HaveFlashlight
    {
        get => _haveFlashlight;
        set
        {
            if (!Equals(_haveFlashlight, value))
            {
                Flashlight.SetActive(value);
                _haveFlashlight = value;
            }
        }
    }

    public delegate void AddItem(InGameItem info, Player Target = Player.First);
    public static event AddItem OnRequestAddingItem;

    public delegate void RemoveItem(InGameItem info, Player target);
    public static event RemoveItem OnRequestRemovingItem;

    public delegate void ManipulateInventory(InventoryUpdateRequirementArgs args);
    public static event ManipulateInventory OnRequestManipulation;
    public static void ForcefullyManipulateInventory(InventoryUpdateRequirementArgs args) =>
        OnRequestManipulation?.Invoke(args);

    public delegate void TransferItem(InGameItem info, Player from, Player to);
    public static event TransferItem OnItemTransfered;
    public static void UpdateItemTransferStatus(InGameItem item, Player from, Player to) =>
        OnItemTransfered?.Invoke(item, from, to);

    public static void RequestAddItem(InGameItem info) => OnRequestAddingItem?.Invoke(info, PlayerSwitcher.Instance.CurrentPlayer);
    public static void RequestAddItem(InGameItem info, Player assigned) => OnRequestAddingItem?.Invoke(info, assigned);

    public static void RequestRemoveItem(InGameItem info, Player assigned) => OnRequestRemovingItem?.Invoke(info, assigned);
    public static void RequestRemoveItem(InGameItem info) => OnRequestRemovingItem?.Invoke(info, PlayerSwitcher.Instance.CurrentPlayer);

    private void Awake()
    {
        //Identify inventory
        if (name == "Player1")
            InventoryOfPlayer = Player.First;
        else
            InventoryOfPlayer = Player.Second;
        PlayerInventory = new List<InGameItem>();
        Flashlight = transform.Find("FlashLight").gameObject;
        Flashlight.SetActive(HaveFlashlight);
    }

    private void OnEnable()
    {
        Debug.Log($"Register event for {name}");
        OnRequestAddingItem += PutOnInventory;
        OnRequestRemovingItem += TakeFromInventory;
        OnRequestManipulation += ManipulatePlayerInventory;
        PlayerSwitcher.OnPlayerChanged += UpdatePlayerChange;
    }

    private void ManipulatePlayerInventory(InventoryUpdateRequirementArgs args)
    {
        Debug.Log($"Attemp manipulate inventory of {InventoryOfPlayer} on {name}\r\n" +
            $"Parameters:\r\n" +
            $"Item name: {args.ItemInfo.Name}\r\n" +
            $"Update request: {args.ItemUpdateType}\r\n" +
            $"Request to player: {args.Specified}\r\n" +
            $"Force?: {args.Forcefully}\r\n" +
            $"Update inventory?: {args.UpdateInventoryUI}");
        switch (args.ItemUpdateType)
        {
            case UpdateType.ItemAdded:
                PlayerInventory.Add(args.ItemInfo);
                Debug.LogWarning($"Add {args.ItemInfo.Name} to {InventoryOfPlayer}'s inventory");
                break;
            case UpdateType.ItemRemoved:
                PlayerInventory.Remove(args.ItemInfo);
                Debug.LogWarning($"Remove {args.ItemInfo.Name} from {InventoryOfPlayer}'s inventory");
                break;
        }
        if (args.UpdateInventoryUI)
            InventoryScreen.Instance.UpdateInventoryItems();
    }

    private void OnDisable()
    {
        OnRequestAddingItem -= PutOnInventory;
        OnRequestRemovingItem -= TakeFromInventory;
    }

    private void UpdatePlayerChange(GameObject player, Player current) => CurrentlyActive = current;

    private void TakeFromInventory(InGameItem info, Player target)
    {
        if (!IsCorrectPlayerInventory(target))
        {
            Debug.LogWarning($"Cancelling attempt to remove {info.Name} from {target}. Wrong player");
            return;
        }
        Debug.Log($"Request removing item {info.Name} from player {target}\r\n" +
            $"in {InventoryOfPlayer} (Current active player: {CurrentlyActive})");
        PlayerInventory.Remove(info);
        InventoryScreen.ForceUpdateInventory();
    }

    private void PutOnInventory(InGameItem info, Player target)
    {
        if (!IsCorrectPlayerInventory(target))
        {
            Debug.LogWarning($"Cancelling attempt to add {info.Name} from {target}. Wrong player");
            return;
        }
        Debug.Log($"Request adding item {info.Name} for player {target} (Current: {InventoryOfPlayer})");
        //Item specification check
        if (info.ItemSepecification == Specific.UnlockFlashlight)
            HaveFlashlight = true;
        //Actually adding item
        PlayerInventory.Add(info);
        //Refresh INV
        InventoryScreen.ForceUpdateInventory();
    }

    public bool IsCorrectPlayerInventory(Player target)
    {
        if (InventoryOfPlayer != CurrentlyActive)
        {
            //Is this right player inventory we're on? NO
            return false;
        }
        if (InventoryOfPlayer != target)
        {
            //Is this the inventory of target player? NO
            return false;
        }
        return true;
    }

}

public class InventoryUpdateRequirementArgs : System.EventArgs
{
    public Player Specified { get; private set; }
    public UpdateType ItemUpdateType { get; private set; }
    public InGameItem ItemInfo { get; private set; }
    public bool Forcefully { get; private set; }
    public bool UpdateInventoryUI { get; private set; }

    public InventoryUpdateRequirementArgs(Player player, InGameItem item, UpdateType update, bool force, bool updateUI)
    {
        ItemInfo = item;
        ItemUpdateType = update;
        Forcefully = force;
        UpdateInventoryUI = updateUI;
    }

    public InventoryUpdateRequirementArgs(InGameItem item, UpdateType update)
    {
        Specified = PlayerSwitcher.Instance.CurrentPlayer;
        ItemInfo = item;
        ItemUpdateType = update;
        Forcefully = false;
        UpdateInventoryUI = true;
    }
}