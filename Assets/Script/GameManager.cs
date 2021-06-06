using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Material LampOn;
    public Material LampOff;

    public Material DeskLampOn;
    public Material DeskLampOff;

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    bool _havePower;
    public bool GlobalElectricityStatus
    {
        get => _havePower;
        set
        {
            if (!Equals(_havePower, value))
            {
                ElectricityChanged?.Invoke(value);
            }
            _havePower = value;
        }
    }

    public delegate void ElectricityStatusUpdate(bool status);
    public static event ElectricityStatusUpdate ElectricityChanged;

    public PlayerState Player1Inventory;
    public PlayerState Player2Inventory;

    public void ShowNotificationOnText(string text, TMP_Text label)
    {
        if (label is null)
            return;
        label.text = text;
        label.DOFade(1, 0.5f);
        label.DOFade(0, 8f);
    }

    public void FlipOnElectricity()
    {
        GlobalElectricityStatus = true;
    }
}
