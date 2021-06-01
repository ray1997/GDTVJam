using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TutorialAInteract : MonoBehaviour
{
    public bool TestTutorialDisplay;
    private void Awake()
    {
        if (TestTutorialDisplay)
            return;
        if (WasSeenTutorialA)
        {
            SelfDestruct();
        }
    }

    bool? _tutorialA = null;
    public bool WasSeenTutorialA
    {
        get
        {
            if (_tutorialA is null)
            {
                _tutorialA = PlayerPrefs.GetInt(nameof(WasSeenTutorialA), 0) == 1;
            }
            Debug.Log($"Player seen tutorial? {_tutorialA}");
            return _tutorialA == true;
        }
        set
        {
            PlayerPrefs.SetInt(nameof(WasSeenTutorialA), value ? 1 : 0);
            _tutorialA = value;
            Debug.Log($"Player seen tutorial? {_tutorialA}");
        }

    }

    public Transform ShowText;
    public TMP_Text UIText;
    bool register;
    public void ShowTutorialText()
    {
        //While in this timeframe, if player is interact immediately destroy itself
        if (!register)
        {
            PlayerInput.OnPlayerInteracted += PlayerDidInteract;
            register = true;
        }
        if (WasSeenTutorialA && !TestTutorialDisplay)
            return;
        if (ShowText != null)
            ShowText.gameObject.SetActive(true);
        if (UIText != null)
        {
            UIText.DOFade(1, 1);
            DOTween.Play(UIText);
        }
        WasSeenTutorialA = true;
        StartCoroutine(SelfDestruct());
    }

    private void PlayerDidInteract()
    {
        StopAllCoroutines();
        DestroyAll();
    }

    public IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(4f);
        DestroyAll();
    }

    public void DestroyAll()
    {
        PlayerInput.OnPlayerInteracted -= PlayerDidInteract;
        Destroy(gameObject);
    }
}