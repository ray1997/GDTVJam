using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookDatabase : MonoBehaviour
{
    public static BookDatabase Instance;

    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public Material[] BookCovers;
}
