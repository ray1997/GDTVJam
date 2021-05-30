using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public delegate void RequestDisableControl();
    public static event RequestDisableControl OnRequestDisableControl;
    public static void ForceTriggerDisabler() => OnRequestDisableControl?.Invoke();

    public delegate void RequestRestoreControl();
    public static event RequestRestoreControl OnRequestRestoreControl;
    public static void ForceTriggerRestorer() => OnRequestRestoreControl?.Invoke();

    [SerializeField]
    public float playerSpeed = 4.0f;

    [SerializeField]
    private float rotationSpeed = 8.0f;

    private float gravityValue = -9.81f;

    private Vector2 inputDirection = Vector2.zero;
    private Vector3 moveAngle, playerVelocity = Vector3.zero;
    private CharacterController controller;
    private Animator animController;

    public static bool GlobalControl = true;

    void Start()
    {
        GlobalControl = true;
        controller = GetComponent<CharacterController>();
        animController = GetComponent<Animator>();
        OnRequestDisableControl += DisablePlayerControls;
        OnRequestRestoreControl += RestorePlayerControls;
    }

    private void RestorePlayerControls()
    {
        if (!gameObject.activeSelf)
            return;
        controller.enabled = true;
        animController.enabled = true;
    }

    private void DisablePlayerControls()
    {
        if (!gameObject.activeSelf)
            return;
        controller.enabled = false;
        animController.enabled = false;
    }

    void Update()
    {
        if (!GlobalControl)
            return;
        // Set moveAngle to match input directions
        moveAngle = new Vector3(inputDirection.x, 0, inputDirection.y);
        moveAngle = Camera.main.transform.forward * moveAngle.z + Camera.main.transform.right * moveAngle.x;
        moveAngle.y = 0f;

        // Set y velocity and move for gravity (add y velocity in future to add jumping)
        playerVelocity.y += gravityValue;
        controller.Move(playerVelocity * Time.deltaTime);

        // Move character in direction of moveAngle, multiply by deltaTime for time-dependency, along with playerSpeed
        controller.Move(moveAngle * Time.deltaTime * playerSpeed);

        // If player is moving, calculate the rotation needed to face that direction, then smoothly rotate using lerp
        if (inputDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnEnable()
    {
        PlayerInput.OnPlayerMovementPerformed += OnPlayerMovementPerformed;
        PlayerInput.OnPlayerMovementCanceled += OnPlayerMovementCanceled;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerMovementPerformed -= OnPlayerMovementPerformed;
        PlayerInput.OnPlayerMovementCanceled -= OnPlayerMovementCanceled;
    }

    private void OnPlayerMovementPerformed(Vector2 direction)
    {
        if (!GlobalControl)
            return;
        inputDirection = direction;
        animController.SetFloat("axis", 1);
    }

    private void OnPlayerMovementCanceled()
    {
        if (!GlobalControl)
            return;
        inputDirection = Vector2.zero;
        animController.SetFloat("axis", 0);
    }
}
