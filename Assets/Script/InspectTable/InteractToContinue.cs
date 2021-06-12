using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractToContinue : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerInput.OnPlayerInteracted += Interacted;
        ObjectiveInfo.OnObjectiveFinished += WaitForQuestFinished;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerInteracted -= Interacted;
        ObjectiveInfo.OnObjectiveFinished -= WaitForQuestFinished;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Interactable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            Interactable = false;
    }

    public bool AllowInteractWin;
    public Quests WaitForQuestID;
    public Quests InteractForQuestID;
    private void WaitForQuestFinished(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (WaitForQuestID == args.FinishedQuest)
        {
            Debug.Log($"Waiting for quest {WaitForQuestID} and it's now finished. {name} can now be interact to win");
            AllowInteractWin = true;
        }
    }

    bool _interactable;
    public bool Interactable
    {
        get => _interactable;
        set => _interactable = value;
    }
    private void Interacted()
    {
        if (!AllowInteractWin)
            return;
        if (!Interactable)
            return;
        Objectives.Instance.MarkQuestAsFinish(InteractForQuestID);
        PlayerInteract?.Invoke();
    }

    public UnityEvent PlayerInteract;
}
