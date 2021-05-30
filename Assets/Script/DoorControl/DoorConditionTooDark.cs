using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DoorConditionTooDark : DoorTrigger
{
    public TMPro.TMP_Text TextToShow;

    public PlayerState _PlayerState;
    public override bool IsLocked()
    {
        DoorStatus = _PlayerState.HaveFlashlight ? LockCondition.Unlocked : LockCondition.LockedNeedCondition;
        if (DoorStatus == LockCondition.LockedNeedCondition)
        {
            if (_PlayerState.HaveFlashlight)
                DoorStatus = LockCondition.Unlocked;
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
