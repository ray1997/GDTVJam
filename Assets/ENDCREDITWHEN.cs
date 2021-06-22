using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ENDCREDITWHEN : MonoBehaviour
{
    public List<Quests> WaitForThese;
    private void OnEnable()
    {
        ObjectiveInfo.OnObjectiveFinished += QuestFinished;
    }

    private void OnDisable()
    {
        ObjectiveInfo.OnObjectiveFinished -= QuestFinished;
    }

    public UnityEvent GameEnded;
    private void QuestFinished(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (WaitForThese.Contains(args.FinishedQuest))
        {
            //Trigger end credit!
            //Or cutscene?
            //Or something!
            GameEnded?.Invoke();
        }
    }
}
