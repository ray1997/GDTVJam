using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RequireUseItem : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        ItemPopulator.OnItemAttempUsage += ListeningToItemUse;
    }

    private void OnTriggerExit(Collider other)
    {
        ItemPopulator.OnItemAttempUsage -= ListeningToItemUse;
    }

    public int QuestID;
    public UnityEvent ItemUsed;
    private void ListeningToItemUse(InGameItem info)
    {
        if (info.ForQuestID != QuestID)
        {
            Debug.Log("Wrong quest, mate");
            return;
        }
        ItemUsed?.Invoke();
    }
}
