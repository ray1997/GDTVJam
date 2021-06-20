using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElevatorQuestMultiValidation : ElevatorQuestValidation
{
    public List<ElevatorWantedUpdateInfo> WantedInfos;
    //public Quests UnlockedQuest;
    //public UnityEvent ElevatorUpdate;
    public override void ItemUpdate(ElevatorControl sender, ElevatorItemUpdatedArgs args)
    {
        foreach (var item in WantedInfos)
        {
            if (args.TriggerItem == item.Item && args.TriggerStatus == item.WantedUpdate)
            {
                item.Done = true;
                break;
            }
        }
        var all = WantedInfos.Where(i => !i.Done).ToList();
        if (all is null)
        {
            Objectives.Instance.MarkQuestAsFinish(UnlockedQuest);
            gameObject.SetActive(false);
        }
        else if (all.Count < 1)
        {
            Objectives.Instance.MarkQuestAsFinish(UnlockedQuest);
            gameObject.SetActive(false);
        }
    }
}

[System.Serializable()]
public class ElevatorWantedUpdateInfo
{
    public InGameItem Item;
    public UpdateType WantedUpdate;
    public bool Done;
}