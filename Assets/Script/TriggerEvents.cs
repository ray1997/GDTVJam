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

    public UnityEvent TriggerStay;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag != "Player")
            return;
        TriggerStay?.Invoke();
    }
}
