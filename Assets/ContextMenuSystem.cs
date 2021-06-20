using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenuSystem : MonoBehaviour
{
    public GameObject UseMenuRoot;
    public GameObject MixMenuRoot;

    public Selectable CurrentSelectionArea;
    public float DelayCloseTime = 0.25f;

    public void EnterArea(Selectable item)
    {
        CurrentSelectionArea = item;
    }

    public void LeaveArea()
    {
        CurrentSelectionArea = null;
        Invoke(nameof(DelayCheck), DelayCloseTime);
    }

    public void DelayCheck()
    {
        if (CurrentSelectionArea is null)
        {
            if (InventoryScreen.Instance.CurrentItemMode == ItemMode.Mix)
                MixMenuRoot.SetActive(false);
            else
                UseMenuRoot.SetActive(false);
        }
    }

    public void ForceClose()
    {
        if (InventoryScreen.Instance.CurrentItemMode == ItemMode.Mix)
            MixMenuRoot.SetActive(false);
        else
            UseMenuRoot.SetActive(false);
    }
}
