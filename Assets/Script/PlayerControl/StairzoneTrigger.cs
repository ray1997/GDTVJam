using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairzoneTrigger : MonoBehaviour
{
    private void OnEnable()
    {
        PlayerSwitcher.OnPlayerChanged += PlayerUpdate;
    }

    private void OnDisable()
    {
        PlayerSwitcher.OnPlayerChanged -= PlayerUpdate;
    }

    private void Awake()
    {
        var player = GameObject.Find("Player1");
        PlayerController = player.GetComponent<Animator>();
        PlayerRigibody = player.GetComponent<Rigidbody>();
        PlayerSpeed = player.GetComponent<PlayerControl>();
    }

    private void PlayerUpdate(GameObject player, Player current)
    {
        PlayerController = player.GetComponent<Animator>();
        PlayerRigibody = player.GetComponent<Rigidbody>();
        PlayerSpeed = player.GetComponent<PlayerControl>();
    }

    public Animator PlayerController;
    public Rigidbody PlayerRigibody;
    public PlayerControl PlayerSpeed;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
            return;
        //Trigger stair animation
        PlayerController.SetBool("stair", true);
        PlayerSpeed.rotationSpeed = 0;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player")
            return;
        //Trigger stair animation
        PlayerController.SetBool("stair", false);
        PlayerSpeed.rotationSpeed = 8f;
    }
}
