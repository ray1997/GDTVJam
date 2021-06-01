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

    public static void RequestAddItem(InGameItem info) => OnRequestAddingItem?.Invoke(info);
    public static void RequestAddItem(InGameItem info, Player assigned) => OnRequestAddingItem?.Invoke(info, assigned);

    private void Awake()
    {
        //Identify inventory
        if (name == "Player1")
            InventoryOfPlayer = Player.First;
        else
            InventoryOfPlayer = Player.Second;
        PlayerInventory = new ObservableCollection<InGameItem>();
        PlayerInventory.CollectionChanged += ItemsUpdated;
        OnRequestAddingItem += PutOnInventory;
        Flashlight = transform.Find("FlashLight").gameObject;
        Flashlight.SetActive(HaveFlashlight);
    }

    private void ItemsUpdated(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
    {
        InventoryScreen.ForceUpdateInventory();
    }

    private void PutOnInventory(InGameItem info, Player target)
    {
        Debug.Log($"Request adding item {info.Name} for player {target} (Current: {InventoryOfPlayer})");
        if (InventoryOfPlayer != target)
            return;
        if (info.ItemSepecification == Specific.UnlockFlashlight)
            HaveFlashlight = true;
        PlayerInventory.Add(info);
    }

    public ObservableCollection<InGameItem> PlayerInventory;
}
