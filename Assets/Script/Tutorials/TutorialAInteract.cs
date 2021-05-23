using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAInteract : MonoBehaviour
{
    private void Awake()
    {
        if (WasSeenTutorialA)
        {
            DestroyItself();
        }
    }

    public void DestroyItself()
    {
        //Remove the script
        Destroy(this);
        Destroy(ShowText.gameObject);
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
            return _tutorialA == true;
        }
        set
        {
            PlayerPrefs.SetInt(nameof(WasSeenTutorialA), value ? 1 : 0);
            _tutorialA = value;
        }

    }

    public Animator ShowText;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (!WasSeenTutorialA)
            {
                ShowText.gameObject.SetActive(true);
                ShowText.SetTrigger("show");
                Invoke(nameof(DestroyItself), 8);
            }
        }
    }
}