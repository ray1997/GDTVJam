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
            var toFind = Interacts.FirstOrDefault(q => q.FinishedID == quest);
            if (toFind != null)
            {
                toFind.gameObject.SetActive(false);
            }
            var cont = InteractContinue.FirstOrDefault(q => q.InteractForQuestID == quest);
            if (cont != null)
            {
                cont.PlayerInteract?.Invoke();
                cont.gameObject.SetActive(false);
            }
            var use = UseItem.FirstOrDefault(q => q.QuestID == quest);
            if (use != null)
            {
                use.ItemUsed?.Invoke();
                use.gameObject.SetActive(false);
            }
            var elevator = ElevatorQuest.FirstOrDefault(q => q.UnlockedQuest == quest);
            if (elevator != null)
            {
                use.gameObject.SetActive(false);
            }
            //
            Objectives.Instance.MarkQuestAsFinish(quest);
            if (quest == Quests.P1FindFlashlight)
                GameManager.Instance.Player1Inventory.HaveFlashlight = true;
            else if (quest == Quests.P2FindFlashlight)
                GameManager.Instance.Player2Inventory.HaveFlashlight = true;
            else if (quest == Quests.P1TurnOnCircuit)
                GameManager.Instance.GlobalElectricityStatus = true;
        }
    }

    private void Start()
    {
        Interacts = FindObjectsOfType<InteractForItem>();
        InteractContinue = FindObjectsOfType<InteractToContinue>();
        UseItem = FindObjectsOfType<RequireUseItem>();
        ElevatorQuest = FindObjectsOfType<ElevatorQuestValidation>();
    }

    //Additional stuff
    InteractForItem[] Interacts;
    InteractToContinue[] InteractContinue;
    RequireUseItem[] UseItem;
    ElevatorQuestValidation[] ElevatorQuest;

    public void SkipQuest(Quests id) => Objectives.Instance.MarkQuestAsFinish(id);

    public void SkipQuest(int id) => Objectives.Instance.MarkQuestAsFinish(id);

    public void SkipQuests(params Quests[] id) => id?.ToList().ForEach(q => Objectives.Instance.MarkQuestAsFinish(q));

    public void SkipQuests(params int[] id) => id?.ToList().ForEach(q => Objectives.Instance.MarkQuestAsFinish(q));
}
