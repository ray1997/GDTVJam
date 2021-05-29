using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCaseRandomizer : MonoBehaviour
{
    public GameObject[] Books;

    private void OnEnable()
    {
        foreach (var book in Books)
        {
            book.SetActive(Random.value > 0.8f);
        }
    }
}
