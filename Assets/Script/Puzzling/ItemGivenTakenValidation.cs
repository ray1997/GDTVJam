using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGivenTakenValidation : MonoBehaviour
{
    public Quests WaitFor;
    public InGameItem LookingForwardFor;
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
        if (info == LookingForwardFor)
        {
            Objectives.Instance.MarkQuestAsFinish(UnlockForGiven);
            Objectives.Instance.MarkQuestAsFinish(UnlockForTaken);
            gameObject.SetActive(false);
        }
    }
}
