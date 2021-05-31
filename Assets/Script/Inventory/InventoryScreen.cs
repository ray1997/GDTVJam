using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScreen : MonoBehaviour
{
    public GameObject InventoryUI;

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
    }
}
