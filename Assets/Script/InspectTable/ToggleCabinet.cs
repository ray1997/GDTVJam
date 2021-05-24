using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCabinet : MonoBehaviour
{
    public Transform OpennedPosition;
    public Transform ClosedPosition;

    public string RegisteredInspectorID;

    private void Awake()
    {
        Physics.queriesHitTriggers = true;
        Inspect.OnBeganInspecting += BeginInspecting;
        Inspect.OnStopInspecting += StopInspecting;
    }

    public bool Inspectable;
    private void StopInspecting(string itemID)
    {
        if (itemID != RegisteredInspectorID)
            return;
        //Disallow inspecting
        Inspectable = false;
        //Reset cabinet
        IsOpen = false;
        transform.position = ClosedPosition.position;
        transform.rotation = ClosedPosition.rotation;
    }

    private void BeginInspecting(string itemID)
    {
        if (itemID != RegisteredInspectorID)
            return;
        //Allow inspecting
        Inspectable = true;
    }

    public bool IsOpen;
    private void OnMouseDown()
    {
        Debug.Log($"Hello! You've clied on item {gameObject.name}");
        ToggleCabinetEvent();
    }

    public void ToggleCabinetEvent()
    {
        Debug.Log($"Trigger cabinet event");
        if (IsOpen)
        {
            StopAllCoroutines();
            MovingObject(transform, ClosedPosition);
        }
        else
        {
            StopAllCoroutines();
            MovingObject(transform, OpennedPosition);
        }
    }

    [Range(0.5f, 3f)]
    public float MovingTime = 1;
    IEnumerator MovingObject(Transform item, Transform target)
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
