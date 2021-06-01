using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UseItemUnselect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public UnityEvent EnterMenu;
    public void OnPointerEnter(PointerEventData eventData)
    {
        EnterMenu?.Invoke();
    }

    public UnityEvent LeaveMenu;
    public void OnPointerExit(PointerEventData eventData)
    {
        LeaveMenu?.Invoke();
    }
}