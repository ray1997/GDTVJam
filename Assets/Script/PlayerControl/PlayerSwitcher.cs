using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSwitcher : MonoBehaviour
{
    public static PlayerSwitcher Instance;
    private void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        PlayerControl.OnRequestDisableControl += DisablePlayerSwitcher;
        PlayerControl.OnRequestRestoreControl += RestorePlayerSwitcher;
    }

    float cachedTime;
    private void RestorePlayerSwitcher()
    {
        NextSwitchAllow = cachedTime;
    }

    private void DisablePlayerSwitcher()
    {
        //Disallow character switch
        cachedTime = NextSwitchAllow;
        NextSwitchAllow = float.MaxValue;
    }

    public GameObject Player1;
    public GameObject Player2;

    public Transform LastPlayer1Location;
    public Transform LastPlayer2Location;

    public delegate void PlayerChanged(GameObject player, Player current);
    public static event PlayerChanged OnPlayerChanged;
    public UnityEvent<Player> ActivePlayerChanged;

    [Range(1f, 60f)]
    public float Cooldown = 3;
    public float NextSwitchAllow;

    public Player CurrentPlayer;
    private void OnEnable()
    {
        PlayerInput.OnPlayerRequestSwitch += SwitchPlayer;
    }

    private void SwitchPlayer()
    {
        if (Time.realtimeSinceStartup < NextSwitchAllow)
            return;
        NextSwitchAllow = Time.realtimeSinceStartup + Cooldown;
        //Sync location
        if (CurrentPlayer == Player.First)
        {
            LastPlayer1Location.position = Camera.main.transform.position;
            LastPlayer1Location.rotation = Camera.main.transform.rotation;
        }
        else
        {
            LastPlayer2Location.position = Camera.main.transform.position;
            LastPlayer2Location.rotation = Camera.main.transform.rotation;
        }
        //Switch
        CurrentPlayer++;
        if (CurrentPlayer > Player.Second)
            CurrentPlayer = Player.First;
        var p1 = Player1.GetComponent<PlayerControl>();
        p1.playerSpeed = CurrentPlayer == Player.First ? p1.walkingSpeed : 0;
        p1.rotationSpeed = CurrentPlayer == Player.First ? 1 : 0;
        p1.tag = CurrentPlayer == Player.First ? "Player" : "OtherPlayer";
        var p2 = Player2.GetComponent<PlayerControl>();
        p2.playerSpeed = CurrentPlayer == Player.Second ? p2.walkingSpeed : 0;
        p2.rotationSpeed = CurrentPlayer == Player.Second ? 1 : 0;
        p2.tag = CurrentPlayer == Player.Second ? "Player" : "OtherPlayer";
        //Send current active player
        OnPlayerChanged?.Invoke(CurrentPlayer == Player.First ? Player1 : Player2, CurrentPlayer);
        //Update Unity Event one as well
        ActivePlayerChanged?.Invoke(CurrentPlayer);
        //Move camera to previous player location
        Camera.main.transform.position =
            CurrentPlayer == Player.First ? LastPlayer1Location.position : LastPlayer2Location.position;
        Camera.main.transform.rotation =
            CurrentPlayer == Player.First ? LastPlayer1Location.rotation : LastPlayer2Location.rotation;
    }
}

public enum Player
{
    First,
    Second,
    Both
}