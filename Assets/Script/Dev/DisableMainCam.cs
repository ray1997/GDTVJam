using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMainCam : MonoBehaviour
{
    GameObject MainPlayer;
    GameObject MainCamera;

    public GameObject FPSControl;
    public bool IsDebugging;
    private void Awake()
    {
        MainPlayer = GameObject.FindGameObjectWithTag("Player");
        MainCamera = Camera.main.gameObject;
        PlayerSwitcher.OnPlayerChanged += UpdateCurrentPlayer;
    }

    private void UpdateCurrentPlayer(GameObject player, Player current)
    {
        MainPlayer = player;
    }

    public void EnableDebugView()
    {
        IsDebugging = !IsDebugging;
        if (IsDebugging)
        {
            //Sync last player position to fps camera position
            FPSControl.transform.position = MainPlayer.transform.position;
            FPSControl.transform.rotation = MainPlayer.transform.rotation;
        }
        else
        {
            MainPlayer.transform.position = FPSControl.transform.position;
            MainPlayer.transform.rotation = FPSControl.transform.rotation;
        }
        FPSControl.SetActive(IsDebugging);
        Cursor.lockState = IsDebugging ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !IsDebugging;
    }

}
