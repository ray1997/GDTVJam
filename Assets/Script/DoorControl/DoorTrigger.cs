using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : MonoBehaviour
{
    public UnityEvent TriggerEntering;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        TriggerEntering?.Invoke();
    }

    public UnityEvent TriggerExiting;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;
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

    public virtual bool IsLocked()
    {
        if (DoorStatus != LockCondition.Unlocked)
            return true;
        return false;
    }

    public LockCondition DoorStatus = LockCondition.Unlocked;
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