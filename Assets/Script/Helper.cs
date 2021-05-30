using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static Helper help;

    private void Awake()
    {
        if (help is null)
            help = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Move item in an instant, no animation
    /// as if teleport
    /// </summary>
    /// <param name="item"></param>
    /// <param name="target"></param>
    public void MoveObject(Transform item, Transform target)
    {
        item.position = target.position;
        item.rotation = target.rotation;
    }

    public void DestroyItem(GameObject item)
    {
        Destroy(item);
    }
}
