using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PauseManager : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerInput.OnRequestTogglePause += TogglePause;
    }

    private void OnDisable()
    {
        PlayerInput.OnRequestTogglePause -= TogglePause;
    }

    public bool IsPause;
    private void TogglePause()
    {
        IsPause = !IsPause;
        DOTween.Clear();
        transform.DOLocalMoveY(IsPause ? 0 : Screen.height * 5, IsPause ? 0.01f : 1);
        transform.DOScale(IsPause ? Vector3.one : Vector3.zero, IsPause ? 0.01f : 1);
        Time.timeScale = IsPause ? 0.01f : 1;
        if (IsPause)
            PlayerControl.ForceTriggerDisabler();
        else
            PlayerControl.ForceTriggerRestorer();
    }

    public void Resume()
    {
        TogglePause();
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }
}
