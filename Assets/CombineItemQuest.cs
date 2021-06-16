using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineItemQuest : MonoBehaviour
{
    //TODO:Add a way to prevent item from combine before quest unlock
    //TODO:Unlock quest after combine
    public bool KeepPrimaryItemAfterCombine;
    public InGameItem PrimaryRequirement;
    public bool KeepSecondaryItemAfterCombine;
    public InGameItem SecondaryRequirement;

    public InGameItem CombineFulfiled;

        #if UNITY_EDITOR
        public bool TestCombine;
        #endif
    void OnEnable()
    {
        #if UNITY_EDITOR
        if (TestCombine)
        {
            PlayerState.RequestAddItem(PrimaryRequirement);
            PlayerState.RequestAddItem(SecondaryRequirement);
        }
        #endif
        InventoryScreen.OnRequestCombineItems += CombineItem;
    }

    void OnDisable()
    {
        InventoryScreen.OnRequestCombineItems -= CombineItem;
    }

    public void CombineItem(InGameItem a, InGameItem b)
    {
        ToastReceiver.ShowToastMessage($"Combining item {a.Name} with {b.Name}");
        //Identify which is primary item
        if ((PrimaryRequirement, SecondaryRequirement) == (a, b) ||
        (PrimaryRequirement, SecondaryRequirement) == (b, a))
        {
            Combine();
        }
    }

    private void Combine()
    {
        if (!KeepPrimaryItemAfterCombine)
            PlayerState.RequestRemoveItem(PrimaryRequirement);
        if (!KeepSecondaryItemAfterCombine)
            PlayerState.RequestRemoveItem(SecondaryRequirement);

        ToastReceiver.ShowToastMessage($"Item combined! You received {CombineFulfiled.Name}");
        PlayerState.RequestAddItem(CombineFulfiled);
        InventoryScreen.CallRequestCombineItemEnd();

        gameObject.SetActive(false);
    }
}