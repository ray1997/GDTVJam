using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorConditionTooDark : DoorTrigger
{
    public TMPro.TMP_Text TextToShow;
    public int WantedQuestID;

    private void Start()
    {
        ObjectiveInfo.OnObjectiveFinished += UnlockDoor;
    }

    private void UnlockDoor(ObjectiveInfo sender, int id)
    {
        if (WantedQuestID == id)
        {
            DoorStatus = LockCondition.Unlocked;
        }
    }

    public override bool IsLocked()
    {
        if (DoorStatus == LockCondition.LockedNeedCondition)
        {
            //Show text and stuff
            TextToShow.DOFade(1, 1);
            Invoke(nameof(HideText), 5f);
        }
        return base.IsLocked();
    }

    void HideText()
    {
        TextToShow.DOFade(0, 1);
    }
}
