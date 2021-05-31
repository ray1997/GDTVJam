using System.Collections;
using System.Collections.Generic;
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
        PlayerInventory = new List<InGameItem>();
        Flashlight = transform.Find("FlashLight").gameObject;
        Flashlight.SetActive(HaveFlashlight);
    }

    private void OnEnable()
    {
        OnRequestAddingItem += PutOnInventory;
    }

    private void PutOnInventory(InGameItem info, Player target)
    {
        if (InventoryOfPlayer != target)
            return;
        if (info.ItemSepecification == Specific.UnlockFlashlight)
            HaveFlashlight = true;
        PlayerInventory.Add(info);
    }

    public List<InGameItem> PlayerInventory;
}
