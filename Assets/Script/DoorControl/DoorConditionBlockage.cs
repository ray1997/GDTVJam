using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DoorConditionBlockage : DoorTrigger
{
    public GameObject Blockage;
    public TMP_Text BlockageNotification;

    public override bool IsLocked()
    {
        if (DoorStatus == LockCondition.LockedBlocked)
        {
            if (Blockage != null)
            {
                if (IsCleared)
                    return false;
                BlockageNotification.DOFade(1, 1);
                Invoke(nameof(HideText), 4f);
                return true;
            }
            else
                return false;
        }
        return base.IsLocked();
    }

    void HideText()
    {
        BlockageNotification.DOFade(0, 1);
    }

    private void OnEnable()
    {
        PlayerInput.OnPlayerInteracted += EtoClear;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerInteracted -= EtoClear;
    }

    [SerializeField] bool _interactable;
    public bool Interactable
    {
        get => _interactable;
        set => _interactable = value;
    }

    public UnityEvent InteractedClear;

    public bool IsCleared;
    private void EtoClear()
    {
        if (!Interactable)
            return;
        if (IsCleared)
            return;
        InteractedClear?.Invoke();
        PlayerInput.OnPlayerInteracted -= EtoClear;
        DoorStatus = LockCondition.Unlocked;
    }
}
