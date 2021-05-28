using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : TriggerEvents
{
    public Animator DoorAnimator;

    public void OpenDoorInward()
    {
        DoorAnimator.SetBool("Inward", true);
        DoorAnimator.SetBool("Open", true);
    }

    public void OpenDoorOutward()
    {
        DoorAnimator.SetBool("Inward", false);
        DoorAnimator.SetBool("Open", true);
    }

    public void CloseDoor()
    {
        DoorAnimator.SetBool("Open", false);
    }
}
