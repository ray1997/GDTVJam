using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class MessageToast : MonoBehaviour
{
    public TMP_Text BackMessage;
    public TMP_Text FrontMessage;
    public RectTransform MSGPane;
    public Image MSGPaneBG;
    public float ShowTime = 5;

    public void ShowToast(string message)
    {
        BackMessage.transform.DOLocalRotate(Vector3.zero, 1);
        BackMessage.text = FrontMessage.text = message;
        StartCoroutine(FadeOutDelete());
    }

    public IEnumerator FadeOutDelete()
    {
        yield return new WaitForSeconds(ShowTime);
        var seq = DOTween.Sequence().
            Append(FrontMessage.DOFade(0, 0.5f)).
            Append(MSGPaneBG.DOFade(0, 0.5f)).
            Append(BackMessage.DOFade(0, 0.5f));
        seq.OnComplete(() => { Destroy(gameObject); });
        seq.Play();
    }
}
