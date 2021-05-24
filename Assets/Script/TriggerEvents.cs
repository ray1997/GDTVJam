using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvents : MonoBehaviour
{
    public UnityEvent TriggerEntering;
    private void OnTriggerEnter(Collider other)
    {
        TriggerEntering?.Invoke();
    }

    public UnityEvent TriggerExiting;
    private void OnTriggerExit(Collider other)
    {
        TriggerExiting?.Invoke();
    }
}
