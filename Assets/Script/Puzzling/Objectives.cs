using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Objectives : MonoBehaviour
{
    public List<ObjectiveInfo> ActiveObjectives;

    public static Objectives Instance;

    public delegate void TriggerQuestFinish(int id, Player forPlayer = Player.First);
    public static event TriggerQuestFinish OnRequestFinished;

    public static void RequestFinishQuest(int id, Player player) => OnRequestFinished?.Invoke(id, player);

    private void Start()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
        PlayerState.OnRequestAddingItem += InventoryCheck;
        ObjectiveInfo.OnObjectiveFinished += ConditionChecks;
        Objectives.OnRequestFinished += SetAsFinish;
    }

    private void SetAsFinish(int id, Player forPlayer = Player.First)
    {
        foreach (var quest in ActiveObjectives)
        {
            if (quest.ID == id && (quest.AssignedPlayer == Player.Both || quest.AssignedPlayer == forPlayer))
            {
                quest.IsDone = true;
            }
        }
    }

    private void ConditionChecks(ObjectiveInfo sender, int id)
    {
        if (ActiveObjectives[0].IsDone == false)
        {
            if (ActiveObjectives[0].IsItAllDone())
            {
                ActiveObjectives[0].IsDone = true;
                ActiveObjectives[1].IsUnlock = true;
            }
        }
    }

    private void InventoryCheck(InGameItem info, Player Target = Player.First)
    {
        if (info.ItemSepecification == Specific.UnlockFlashlight)
        {
            //Unlock next objective?
            if (Target == Player.First)
            {
                ActiveObjectives[0].SubObjective[0].IsDone = true;
            }
            else if (Target == Player.Second)
            {
                ActiveObjectives[0].SubObjective[1].IsDone = true;
            }            
        }
    }
}
