using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractForItem : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerInput.OnPlayerInteracted += PressedE;
        if (NeedQuest)
        {
            ObjectiveInfo.OnObjectiveFinished += QuestFinished;
            VisualItem.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void QuestFinished(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (args.FinishedQuest == QuestName)
        {
            VisualItem.SetActive(true);
            GetComponent<BoxCollider>().enabled = true;
        }
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerInteracted -= PressedE;
        if (NeedQuest)
            ObjectiveInfo.OnObjectiveFinished -= QuestFinished;
    }

    private void PressedE()
    {
        if (!WithinRange)
            return;
        //Give item if within range
        PlayerState.RequestAddItem(GivenItem, PlayerSwitcher.Instance.CurrentPlayer);
        //Hide item and trigger
        VisualItem.SetActive(false);
        gameObject.SetActive(false);
        if (ForQuest)
            Objectives.Instance.MarkQuestAsFinish((int)FinishedID);
        InteractedWon?.Invoke();
    }

    public InGameItem GivenItem;
    public GameObject VisualItem;

    public bool WithinRange;

    public bool NeedQuest;
    public Quests QuestName;

    public bool ForQuest;
    public Quests FinishedID;

    public UnityEvent InteractedWon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            WithinRange = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            WithinRange = false;
    }
}
