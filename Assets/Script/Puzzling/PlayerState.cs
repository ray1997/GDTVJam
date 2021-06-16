using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    Player InventoryOfPlayer;
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
        OnRequestAddingItem += PutOnInventory;
        OnRequestRemovingItem += TakeFromInventory;
        PlayerSwitcher.OnPlayerChanged += UpdatePlayerChange;
    }

    private void OnDisable()
    {
        OnRequestAddingItem -= PutOnInventory;
        OnRequestRemovingItem -= TakeFromInventory;
    }

    private void UpdatePlayerChange(GameObject player, Player current) => InventoryOfPlayer = current;

    private void TakeFromInventory(InGameItem info, Player target)
    {
        Debug.Log($"Request removing item {info.Name} for player {target} (Current: {InventoryOfPlayer})");
        if (InventoryOfPlayer != target)
            return;
        PlayerInventory.Remove(info);
    }

    private void PutOnInventory(InGameItem info, Player target)
    {
        Debug.Log($"Request adding item {info.Name} for player {target} (Current: {InventoryOfPlayer})");
        if (InventoryOfPlayer != target)
        {
            return;
        }
        else
        {
            Debug.Log($"Adding item {info.Name} for player {target}");
            if (info.ItemSepecification == Specific.UnlockFlashlight)
                HaveFlashlight = true;
            PlayerInventory.Add(info);
            InventoryScreen.ForceUpdateInventory();
        }
    }

    public List<InGameItem> PlayerInventory;
}
