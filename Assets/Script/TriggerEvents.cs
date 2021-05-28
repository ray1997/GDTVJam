using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
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

    public GameObject RotatedObject;
    [Range(0.25f, 5)]
    public float RotateDuration = 1f;
    public void RotateObject(float Angle)
    {
        //RotatedObject.transform.Rotate(new Vector3(0, 0, Angle));
        StartCoroutine(SlowlyRotateTo(RotatedObject.transform, Angle));
    }

    public IEnumerator SlowlyRotateTo(Transform wantedObject, float angle)
    {
        float begin = 0;
        if (angle > 1 || angle < -1)
        {
            Quaternion origin = wantedObject.rotation;
            while (begin < RotateDuration)
            {
                wantedObject.rotation = Quaternion.Lerp(origin, Quaternion.Euler(0, 0, angle), begin / RotateDuration);
                begin += Time.smoothDeltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            Quaternion origin = wantedObject.rotation;
            while (begin < RotateDuration)
            {
                wantedObject.rotation = Quaternion.Lerp(origin, Quaternion.Euler(0, 0, 0), begin / RotateDuration);
                begin += Time.smoothDeltaTime;
                yield return new WaitForEndOfFrame();
            }
        }
        yield return new WaitForEndOfFrame();
    }
}
