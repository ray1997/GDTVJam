using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestSkip : MonoBehaviour
{
    public Quests[] Skips;

    public void SkipQuest()
    {
        //SKIP
        Debug.LogError("REQUEST FORCEFULLY MARK QUEST AS COMPLETE");
        foreach (var quest in Skips)
        {
            Objectives.Instance.MarkQuestAsFinish(quest);
            if (quest == Quests.P1FindFlashlight)
                GameManager.Instance.Player1Inventory.HaveFlashlight = true;
            else if (quest == Quests.P2FindFlashlight)
                GameManager.Instance.Player2Inventory.HaveFlashlight = true;
            else if (quest == Quests.P1TurnOnCircuit)
                GameManager.Instance.GlobalElectricityStatus = true;
        }
    }

    public void SkipQuest(Quests id) => Objectives.Instance.MarkQuestAsFinish(id);

    public void SkipQuest(int id) => Objectives.Instance.MarkQuestAsFinish(id);

    public void SkipQuests(params Quests[] id) => id?.ToList().ForEach(q => Objectives.Instance.MarkQuestAsFinish(q));

    public void SkipQuests(params int[] id) => id?.ToList().ForEach(q => Objectives.Instance.MarkQuestAsFinish(q));
}
