using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspect : MonoBehaviour
{
    public delegate void BeginInspect(string itemID);
    public static event BeginInspect OnBeganInspecting;

    public delegate void StopInspect(string itemID);
    public static event StopInspect OnStopInspecting;

    public GameObject InspectCameraPosition;
    Transform OriginalCamPosition;

    public string InspectID = "";

    public void InspectTable()
    {
        StopAllCoroutines();
        //Save original cam position
        OriginalCamPosition = Camera.main.transform;
        //Lerp camera to that inspect position
        StartCoroutine(MovingCamera(Camera.main.transform, InspectCameraPosition.transform));

        OnBeganInspecting?.Invoke(InspectID);
    }

    public void LeaveTable()
    {
        StopAllCoroutines();
        //Restore camera position
        StartCoroutine(MovingCamera(Camera.main.transform, OriginalCamPosition));

        OnStopInspecting?.Invoke(InspectID);
    }

    [Range(0.5f, 3)]
    public float MovingTime = 1;
    IEnumerator MovingCamera(Transform item, Transform target)
    {
        float moving = 0;
        Vector3 cachedBeginP = item.position;
        Quaternion cachedBeginR = item.rotation;
        while (moving < MovingTime)
        {
            item.position = Vector3.Lerp(cachedBeginP, target.position, (moving / MovingTime));
            item.rotation = Quaternion.Lerp(cachedBeginR, target.rotation, (moving / MovingTime));
            moving += Time.smoothDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        //Incase of extra -seconds
        item.position = target.position;
        item.rotation = target.rotation;
        yield return new WaitForFixedUpdate();
    }
}
