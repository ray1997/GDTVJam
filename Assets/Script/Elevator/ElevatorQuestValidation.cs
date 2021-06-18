using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ElevatorQuestValidation : MonoBehaviour
{
    private void OnEnable()
    {
        ElevatorControl.OnElevatorItemChanged += ItemUpdate;
    }

    public InGameItem WantedItem;
    public UpdateType WantedUpdate;
    public Quests UnlockedQuest;
    public UnityEvent ElevatorUpdate;
    private void ItemUpdate(ElevatorControl sender, ElevatorItemUpdatedArgs args)
    {
        if (args.TriggerItem == WantedItem && args.TriggerStatus == WantedUpdate)
        {
            Debug.Log($"Elevator update match requirement {WantedItem.Name} [{WantedUpdate}]");
            if (Objectives.Instance.IsQuestUnlock((int)UnlockedQuest))
            {
                Objectives.Instance.MarkQuestAsFinish(UnlockedQuest);
                ElevatorUpdate?.Invoke();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        ElevatorControl.OnElevatorItemChanged -= ItemUpdate;
    }

}