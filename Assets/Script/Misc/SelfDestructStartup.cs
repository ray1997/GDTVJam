using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructStartup : MonoBehaviour
{
    public bool Prevent;
    private void Awake()
    {
        if (Prevent)
            return;
        Destroy(gameObject);
    }
}
