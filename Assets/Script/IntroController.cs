using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroController : MonoBehaviour
{
    public bool SkipIntro;

    public GameObject PlayerIntroController;
    public GameObject Player;
    void Awake()
    {
        if (SkipIntro)
        {
            Player.SetActive(true);
            PlayerIntroController.SetActive(false);
        }
    }
}
