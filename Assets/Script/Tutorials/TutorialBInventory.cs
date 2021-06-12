using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialBInventory : MonoBehaviour
{
    public TMP_Text DisplayText;
    public PlayerState Player1Inv;

    private void OnEnable()
    {
        ObjectiveInfo.OnObjectiveFinished += WaitForQuestToSelfDestruct;
    }

    private void WaitForQuestToSelfDestruct(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (args.FinishedQuest == Quests.P1FindFuse)
            DisplayText.text = "You found a fuse!\r\nPress [Q] to open inventory\r\nThen use fuse it to replace the broken one";
        else if (args.FinishedQuest == Quests.P1ReplaceFuse)
            DisplayText.text = "Before you activate the circuit, Go fix elevator first!\r\nPress [Space] to switch to your friend!";
        else if (args.FinishedQuest == Quests.P1TurnOnCircuit)
        {
            DisplayText.text = "Lights on!";
            Invoke(nameof(DelayDisable), 5);
        }
    }

    public void DelayDisable() => TutorialRoot.SetActive(false);
    public GameObject TutorialRoot;

    private void OnDisable()
    {
        ObjectiveInfo.OnObjectiveFinished -= WaitForQuestToSelfDestruct;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Check inventory for context
        var fuse = Player1Inv.PlayerInventory.FirstOrDefault(i => i.Name == "Fuse");
        if (fuse is null)
        {
            if (Objectives.Instance.IsQuestFinish((int)Quests.P1ReplaceFuse))
                DisplayText.text = "Fuse replaced, ready to switch on!";
            else
                DisplayText.text = "It's appear that the fuse is busted.\r\nGo find a replacement fuse";
        }
        else
            DisplayText.text = "You found a fuse!\r\nPress [Q] to open inventory\r\nThen use fuse it to replace the broken one";
        if (other.tag == "Player")
            DisplayText.DOFade(1, 1);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            DisplayText.DOFade(0, 1);
    }
}
