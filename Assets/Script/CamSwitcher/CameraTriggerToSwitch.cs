using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerToSwitch : MonoBehaviour
{
    public Transform CameraTransform;
    public Transform PlayerTransform;
    private void Awake()
    {
        CameraTransform = Camera.main.transform;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public Transform CameraPointA;
    public void SwitchToPointA()
    {
        if (CameraTransform.position == CameraPointA.position) //Immediately switch other one on?
            TriggerB.SetActive(true);
        MoveObject(CameraTransform, CameraPointA);
    }

    public Transform CameraPointB;
    public void SwitchToPointB()
    {
        if (CameraTransform.position == CameraPointB.position) //Immediately switch other one on?
            TriggerA.SetActive(true);
        MoveObject(CameraTransform, CameraPointB);
    }

    public void MoveObject(Transform item, Transform target)
    {
        item.position = target.position;
        item.rotation = target.rotation;
    }

    public GameObject TriggerA;
    public GameObject TriggerB;
    public bool RestoreAllTriggers;
    public virtual void RestoreTrigger()
    {
        if (RestoreAllTriggers)
        {
            TriggerA.SetActive(true);
            TriggerB.SetActive(true);
            return;
        }
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

    public void UpdateTriggerPosition()
    {
        //Check which trigger furthest from player
        float a = Vector3.Distance(PlayerTransform.position, TriggerA.transform.position);
        float b = Vector3.Distance(PlayerTransform.position, TriggerB.transform.position);
        
        if (a > b)
        {
            //Player is leaving trigger B?
            TriggerA.SetActive(true);
        }
        else if (b > a)
        {
            //Player is leaving trigger A?
            TriggerB.SetActive(true);
        }
    }
}
