using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EndCreditRoller : MonoBehaviour
{
    private void OnEnable()
    {
        //Begin animating
        EndCreditRollerTweens();
    }

    public Image BlackBackground;
    public RectTransform Panel;
    public RectTransform CreditText;

    public float BeginScroll = -800f;
    public float EndScroll = 1200f;
    public float ScrollTime = 15f;
    public void EndCreditRollerTweens()
    {
        var seq = DOTween.Sequence().
            Append(BlackBackground.DOFade(1, 3)).
            Append(Panel.DOLocalMoveY(EndScroll, ScrollTime));
        seq.OnComplete(() => { BackToMainMenu(); });
        seq.Play();
    }

    public void BackToMainMenu()
    {
        //Do leave?
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit(68421);   
#endif
    }
}
