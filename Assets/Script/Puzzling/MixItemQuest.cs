using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MixItemQuest : MonoBehaviour
{
    public bool CanMix;
    public Quests WaitFor;
    public Quests Finish;

    public List<MixerInfo> MixRequire;

    public InGameItem MixFulfiled;

    void OnEnable()
    {
        InventoryScreen.OnFinalizeMix += MixItems;
        ObjectiveInfo.OnObjectiveFinished += MonitoringQuest;
    }

    void OnDisable()
    {
        InventoryScreen.OnFinalizeMix -= MixItems;
        ObjectiveInfo.OnObjectiveFinished -= MonitoringQuest;
    }

    public void MonitoringQuest(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (args.FinishedQuest == WaitFor)
            CanMix = true;
    }

    private void MixItems(List<InGameItem> items)
    {
        ToastReceiver.ShowToastMessage($"Mixing items {string.Join(",",items.Select(i => i.Name))}");
        foreach (var item in items)
        {
            var mix = MixRequire.FindIndex(i => i.ItemInfo == item);
            if (mix >= 0)
            {
                MixRequire[mix].Got = true;
            }
        }
        //Check if player got all ingredient
        var all = MixRequire.Where(i => !i.Got).ToList();
        if (all is null || all.Count < 1)
        {
            //Got it all!
            MixItem();
        }
        else
        {
            ToastReceiver.ShowToastMessage("Incomplete ingredient. Mixing cancelled");
        }
    }

    public void MixItem()
    {
        foreach (var item in MixRequire)
        {
            if (!item.KeepIt)
                PlayerState.RequestRemoveItem(item.ItemInfo);
        }
        ToastReceiver.ShowToastMessage($"Item mixed! You received {MixFulfiled.Name}");
        PlayerState.RequestAddItem(MixFulfiled);

        gameObject.SetActive(false);

        Objectives.Instance.MarkQuestAsFinish(Finish);
    }
}

[System.Serializable()]
public class MixerInfo
{
    public InGameItem ItemInfo;
    public bool KeepIt;
    public bool Got;
}