using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RequireUseItem : MonoBehaviour
{
    private void OnEnable()
    {
        if (WaitForQuest)
            ObjectiveInfo.OnObjectiveFinished += Finished;
    }

    private void OnDisable()
    {
        if (WaitForQuest)
            ObjectiveInfo.OnObjectiveFinished -= Finished;
    }

    private void Finished(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (Waited == args.FinishedQuest)
            Allow = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        ItemPopulator.OnItemAttempUsage += ListeningToItemUse;
    }

    private void OnTriggerExit(Collider other)
    {
        ItemPopulator.OnItemAttempUsage -= ListeningToItemUse;
    }

    public Quests Waited;
    public bool WaitForQuest;
    public bool Allow;

    public Quests QuestID;
    public UnityEvent ItemUsed;
    private void ListeningToItemUse(InGameItem info)
    {
        if (WaitForQuest && !Allow)
        {
            Debug.Log($"Wait for quest {Waited} to finish");
            return;
        }
        if (!info.ForQuests.Contains(QuestID))
        {
            ToastReceiver.ShowToastMessage($"You tried to use {info.Name}\r\nIt's not working");
            Debug.Log("Wrong quest, mate");
            return;
        }
        else
            ToastReceiver.ShowToastMessage($"You tried to use {info.Name}\r\nIt's work!");
        //Activate quest finish
        Objectives.Instance.MarkQuestAsFinish(QuestID);
        //Request remove item
        PlayerState.RequestRemoveItem(info, PlayerSwitcher.Instance.CurrentPlayer);
        //
        ItemUsed?.Invoke();
    }
}
