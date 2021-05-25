using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerToSwitch : MonoBehaviour
{
    Transform CameraTransform;
    Transform PlayerTransform;
    private void Awake()
    {
        CameraTransform = Camera.main.transform;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Transform CameraPointA;
    public void SwitchToPointA()
    {
        MoveObject(CameraTransform, CameraPointA);
    }

    public Transform CameraPointB;
    public void SwitchToPointB()
    {
        MoveObject(CameraTransform, CameraPointB);
    }

    public void MoveObject(Transform item, Transform target)
    {
        item.position = target.position;
        item.rotation = target.rotation;
    }

    public GameObject TriggerA;
    public GameObject TriggerB;
    public void RestoreTrigger()
    {
        //Check which trigger furthest from player
        float a = Vector3.Distance(PlayerTransform.position, TriggerA.transform.position);
        float b = Vector3.Distance(PlayerTransform.position, TriggerB.transform.position);

        if (a > b)
        {
            //Player is leaving trigger B?
            TriggerA.SetActive(true);
            //Restore camera to that point
            MoveObject(CameraTransform, CameraPointB);
        }
        else if (b > a)
        {
            //Player is leaving trigger A?
            TriggerB.SetActive(true);
            //Restore to B
            MoveObject(CameraTransform, CameraPointA);
        }
    }
}
