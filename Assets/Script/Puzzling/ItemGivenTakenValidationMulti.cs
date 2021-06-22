using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemGivenTakenValidationMulti : MonoBehaviour
{
    public Quests WaitFor;
    public List<ElevatorWantedUpdateInfo> LookingForwardFor;
    public Quests UnlockForGiven;
    public Quests UnlockForTaken;

    private void OnEnable()
    {
        PlayerState.OnItemTransfered += TranferMonitoring;
    }

    private void OnDisable()
    {
        PlayerState.OnItemTransfered += TranferMonitoring;
    }

    private void TranferMonitoring(InGameItem info, Player from, Player to)
    {
        for (int i = 0; i < LookingForwardFor.Count; i++)
        {
            if (info == LookingForwardFor[i].Item)
            {
                LookingForwardFor[i].Done = true;
            }
        }
        var all = LookingForwardFor.Where(i => !i.Done).ToList();
        if (all is null || all.Count < 1)
            return;
        Objectives.Instance.MarkQuestAsFinish(UnlockForGiven);
        Objectives.Instance.MarkQuestAsFinish(UnlockForTaken);
        gameObject.SetActive(false);
    }
}