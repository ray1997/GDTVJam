using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BookDatabase;

public class BookCaseRandomizer : MonoBehaviour
{
    [SerializeField] MeshRenderer[] RowRender;
    [SerializeField] MeshRenderer[] BookRender;

    public bool[] Lock;

    private void OnEnable()
    {
        Invoke(nameof(Shuffle), Random.Range(2, 5));
    }

    public void Shuffle()
    {
        for (int i = 0; i < RowRender.Length; i++)
        {
            if (Lock != null)
                if (Lock.Length > i)
                    if (Lock[i])
                        continue;
            int random = Random.Range(0, Instance.BookCovers.Length);
            RowRender[i].material = Instance.BookCovers[random];
            BookRender[i].material = Instance.BookCovers[random];
        }
    }
}
