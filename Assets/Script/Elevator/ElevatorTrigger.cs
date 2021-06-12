using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTrigger : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        ElevatorControl.Instance.FocusTo(other, name);
    }

    private void OnTriggerExit(Collider other)
    {
        ElevatorControl.Instance.LeftArea(other, name);
    }
}
