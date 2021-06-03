using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class DoorTrigger : MonoBehaviour
{
    public LockCondition DoorStatus = LockCondition.Unlocked;

    public TMPro.TMP_Text TextToShow;

    private void OnEnable()
    {
        if (DoorStatus == LockCondition.LockedNeedCondition)
            ObjectiveInfo.OnObjectiveFinished += UnlockDoor;
        if (DoorStatus == LockCondition.LockedBlocked)
            PlayerInput.OnPlayerInteracted += InteractClearBlockage;
    }

    private void OnDisable()
    {
        if (DoorStatus == LockCondition.LockedNeedCondition)
            ObjectiveInfo.OnObjectiveFinished -= UnlockDoor;
        if (DoorStatus == LockCondition.LockedBlocked)
            PlayerInput.OnPlayerInteracted -= InteractClearBlockage;
    }

    #region Conditional
    public Quests WantedQuestID;
    #endregion

    #region Blockage
    public bool Interactable;
    public GameObject Blockage;
    public UnityEvent InteractedClear;
    private void InteractClearBlockage()
    {
        if (!Interactable)
            return;
        if (IsCleared)
            return;
        InteractedClear?.Invoke();
        PlayerInput.OnPlayerInteracted -= InteractClearBlockage;
        DoorStatus = LockCondition.Unlocked;
    }

    #endregion

    private void UnlockDoor(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        Debug.Log($"Request unlock door as quest [{(int)args.FinishedQuest}]{args.FinishedQuest} has finished." +
            $"\r\nUnlock {(WantedQuestID == args.FinishedQuest ? "allowed" : "disallowed")}." +
            $"\r\nWaiting for quest ID {WantedQuestID}. Finished quest ID is {args.FinishedQuest}");
        if (WantedQuestID == args.FinishedQuest)
        {
            DoorStatus = LockCondition.Unlocked;
        }
    }

    public UnityEvent TriggerEntering;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        Interactable = true;
        TriggerEntering?.Invoke();
    }

    public UnityEvent TriggerExiting;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;
        Interactable = false;
        TriggerExiting?.Invoke();
    }

    public Animator DoorAnimator;

    public void OpenDoorInward()
    {
        if (IsLocked())
            return;
        DoorAnimator.SetBool("Inward", true);
        DoorAnimator.SetBool("Open", true);
    }

    public void OpenDoorOutward()
    {
        if (IsLocked())
            return;
        DoorAnimator.SetBool("Inward", false);
        DoorAnimator.SetBool("Open", true);
    }

    public void CloseDoor()
    {
        DoorAnimator.SetBool("Open", false);
    }

    public bool IsCleared;
    public virtual bool IsLocked()
    {
        if (DoorStatus == LockCondition.LockedNeedCondition)
        {
            TextToShow.DOFade(1, 1);
            Invoke(nameof(HideText), 5f);
        }
        if (DoorStatus == LockCondition.LockedBlocked)
        {
            if (Blockage != null)
            {
                if (IsCleared)
                    return false;
                TextToShow.DOFade(1, 1);
                Invoke(nameof(HideText), 4f);
                return true;
            }
        }
        if (DoorStatus != LockCondition.Unlocked)
            return true;
        return false;
    }

    void HideText()
    {
        TextToShow.DOFade(0, 1);
    }

}

public enum LockCondition
{
    /// <summary>
    /// Not lock
    /// </summary>
    Unlocked,
    /// <summary>
    /// Need to meet some condition first
    /// </summary>
    LockedNeedCondition,
    /// <summary>
    /// Need certain item on inventory first
    /// </summary>
    LockedNeedItem,
    /// <summary>
    /// Lock because something block on the other side
    /// </summary>
    LockedBlocked
}