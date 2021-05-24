using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CabinetControl : MonoBehaviour
{
    bool _open;
    public bool IsOpen
    {
        get => _open;
        set
        {
            if (!Equals(_open, value))
            {
                if (value)
                    OpenCabinet();
                else
                    CloseCabinet();
                _open = value;
            }
        }
    }

    public Transform OpenPosition;
    public Transform ClosePosition;

    public void OpenCabinet()
    {
        StopAllCoroutines();
        StartCoroutine(MovingItem(transform, OpenPosition));
    }

    public void CloseCabinet()
    {
        StopAllCoroutines();
        StartCoroutine(MovingItem(transform, ClosePosition));
    }

    [Range(0.5f, 3)]
    public float MovingTime = 1;
    IEnumerator MovingItem(Transform item, Transform target)
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
