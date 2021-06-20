using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombineItemQuest : MonoBehaviour
{
    public bool CanCombine;
    public Quests WaitFor;
    public Quests Finish;
    public bool KeepPrimaryItemAfterCombine;
    public InGameItem PrimaryRequirement;
    public bool KeepSecondaryItemAfterCombine;
    public InGameItem SecondaryRequirement;

    public InGameItem CombineFulfiled;

    void OnEnable()
    {
        InventoryScreen.OnRequestCombineItems += CombineItem;
        ObjectiveInfo.OnObjectiveFinished += MonitoringQuest;
    }

    void OnDisable()
    {
        InventoryScreen.OnRequestCombineItems -= CombineItem;
        ObjectiveInfo.OnObjectiveFinished -= MonitoringQuest;
    }

    public void MonitoringQuest(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (args.FinishedQuest == WaitFor)
            CanCombine = true;
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

        Objectives.Instance.MarkQuestAsFinish(Finish);
    }
}