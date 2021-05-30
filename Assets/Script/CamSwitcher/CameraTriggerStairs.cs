using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerStairs : CameraTriggerToSwitch
{
    //Triggers
    public GameObject EnterAreaFirstFloorStair;
    public GameObject EnterAreaSecondFloorStair;

    public GameObject LeaveStairsArea;

    //Just put a bunches of cameras. Whatever close to just use that.
    public List<Transform> Cameras;

    [SerializeField]
    public bool UpdatingCameras { get; set; }
    Transform ClosestCam;
    private void Update()
    {
        if (!UpdatingCameras)
            return;
        Vector3 playerPos = PlayerTransform.position;
        float closestDis = 100;
        //Constantly move to closest cameras;
        foreach (var cam in Cameras)
        {
            float distance = Vector3.Distance(cam.position, playerPos);
            if (distance < closestDis)
            {
                ClosestCam = cam;
                closestDis = distance;
            }
        }
        if (CameraTransform.position != ClosestCam.position)
            Helper.help.MoveObject(CameraTransform, ClosestCam);
    }

    public void SwitchToFirstFloorCamera()
    {
        UpdatingCameras = true;
        PlayerControl.ForceTriggerDisabler();
        Invoke("RestoreControl", 0.5f);
    }

    public void RestoreControl() => PlayerControl.ForceTriggerRestorer();

    public void SwitchToSecondFloorCamera()
    {
        UpdatingCameras = true;
        PlayerControl.ForceTriggerDisabler();
        Invoke("RestoreControl", 0.5f);
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
        //Enable further room trigger
        Vector3 playerPos = PlayerTransform.position;
        float a = Vector3.Distance(playerPos, TriggerA.transform.position);
        float b = Vector3.Distance(playerPos, TriggerB.transform.position);
        if (a > b)
            Helper.help.MoveObject(CameraTransform, CameraPointB);
        else
            Helper.help.MoveObject(CameraTransform, CameraPointA);
        //
        UpdatingCameras = false;
    }
}
