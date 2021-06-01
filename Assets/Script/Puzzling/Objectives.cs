using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Objectives : MonoBehaviour
{
    public List<ObjectiveInfo> ActiveObjectives;

    public static Objectives Instance;

    private void Start()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
        PlayerState.OnRequestAddingItem += InventoryCheck;
        ObjectiveInfo.OnObjectiveFinished += UnlockableCheck;
    }

    private void UnlockableCheck(ObjectiveInfo sender, int id)
    {
        if (sender.IsDone && sender.Unlockable?.Count > 0)
        {
            //Unlock quests
            foreach (var quest in ActiveObjectives)
            {
                if (sender.Unlockable.Contains(quest.ID))
                {
                    quest.IsUnlock = true;
                }
            }
        }
    }

    private void InventoryCheck(InGameItem info, Player Target = Player.First)
    {
        if (info.ItemSepecification == Specific.UnlockFlashlight)
        {

        }
    }

    public bool IsQuestFinish(int id)
    {
        foreach (var quest in ActiveObjectives)
        {
            if (quest.ID == id)
            {
                return quest.IsDone;
            }
        }
        return false;
    }
    public bool IsQuestUnlock(int id)
    {
        foreach (var quest in ActiveObjectives)
        {
            if (quest.ID == id)
            {
                return quest.IsUnlock;
            }
        }
        return false;
    }

    public void MarkQuestAsFinish(int id)
    {
        Debug.Log("Try to mark quest ID " + id + "as finished");
        var quest = ActiveObjectives.FirstOrDefault(q => q.ID == id);
        Debug.Log($"Attempt {(quest is null ? "failed" : "working, found quest with that ID")}");
        if (quest != null)
        {
            quest.IsDone = true;
        }
    }
}
