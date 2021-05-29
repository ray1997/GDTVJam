using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerStairs : CameraTriggerToSwitch
{
    //Triggers
    public GameObject EnterAreaFirstFloorStair;
    public GameObject EnterAreaSecondFloorStair;

    public GameObject LeaveStairsArea;

    public Transform CameraFirstFloor;
    public Transform CameraSecondFloor;

    public void SwitchToFirstFloorCamera()
    {
        MoveObject(CameraTransform, CameraFirstFloor);
    }

    public void SwitchToSecondFloorCamera()
    {
        MoveObject(CameraTransform, CameraSecondFloor);
    }

    public void LeftCameraAreaFirstFloor()
    {
        SwitchToPointA();
    }

    public void LeftCameraAreaSecondFloor()
    {
        SwitchToPointB();
    }

    public override void RestoreTrigger()
    {
        //Set a/b triggers off
        TriggerA.SetActive(false);
        TriggerB.SetActive(false);
        //Restore stairs trigger
        EnterAreaFirstFloorStair.SetActive(true);
        EnterAreaSecondFloorStair.SetActive(true);
    }
}
