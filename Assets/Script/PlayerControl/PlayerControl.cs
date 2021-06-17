using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    public Player ControlFor;
    public Player Current;


    //public delegate void RequestDisableControl();
    public delegate void RequestDisableControl();

    public static event RequestDisableControl OnRequestDisableControl;
    public static void ForceTriggerDisabler() => OnRequestDisableControl?.Invoke();

    public delegate void RequestRestoreControl();
    public static event RequestRestoreControl OnRequestRestoreControl;
    public static void ForceTriggerRestorer() => OnRequestRestoreControl?.Invoke();

    public float walkingSpeed = 1.5f;
    public float runningSpeed = 4f;

    public float playerSpeed = 4.0f;

    public float rotationSpeed = 8.0f;

    private float gravityValue = -9.81f;

    private Vector2 inputDirection = Vector2.zero;
    private Vector3 moveAngle, playerVelocity = Vector3.zero;
    private CharacterController controller;
    private Animator animController;

    public static bool GlobalControl = true;

    public List<GameObject> PlayerParts;
    void Start()
    {
        GlobalControl = true;
        controller = GetComponent<CharacterController>();
        animController = GetComponent<Animator>();
        OnRequestDisableControl += DisablePlayerControls;
        OnRequestRestoreControl += RestorePlayerControls;
        foreach (Transform t in transform)
        {
            if (t.name.StartsWith("FlashLight"))
                continue;
            PlayerParts.Add(t.gameObject);
        }
    }

    private void RestorePlayerControls()
    {
        if (!gameObject.activeSelf)
            return;
        controller.enabled = true;
        animController.enabled = true;
        PlayerParts.ForEach(g => g.SetActive(true));
        PlayerInput.OnPlayerMovementPerformed += OnPlayerMovementPerformed;
        PlayerInput.OnPlayerMovementCanceled += OnPlayerMovementCanceled;
        GlobalControl = true;
    }

    private void DisablePlayerControls()
    {
        //disable both player!
        PlayerInput.OnPlayerMovementPerformed -= OnPlayerMovementPerformed;
        PlayerInput.OnPlayerMovementCanceled -= OnPlayerMovementCanceled;
        PlayerInput.OnPlayerStartRunning -= StartRunning;
        PlayerInput.OnPlayerStopRunning -= StopRunning;
        PlayerSwitcher.OnPlayerChanged -= UpdatePlayerInfo;
    }

    void Update()
    {
        if (!GlobalControl)
            return;
        if (Current != ControlFor)
            return;

        // Set moveAngle to match input directions
        moveAngle = new Vector3(inputDirection.x, 0, inputDirection.y);
        moveAngle = PreviousCameraTransform.forward * moveAngle.z + PreviousCameraTransform.right * moveAngle.x;
        moveAngle.y = 0f;

        // Set y velocity and move for gravity (add y velocity in future to add jumping)
        playerVelocity.y += gravityValue;
        controller.Move(playerVelocity * Time.deltaTime);

        // Move character in direction of moveAngle, multiply by deltaTime for time-dependency, along with playerSpeed
        controller.Move(moveAngle * Time.deltaTime * playerSpeed);

        // If player is moving, calculate the rotation needed to face that direction, then smoothly rotate using lerp
        if (inputDirection != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + PreviousCameraTransform.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnEnable()
    {
        PlayerInput.OnPlayerMovementPerformed += OnPlayerMovementPerformed;
        PlayerInput.OnPlayerMovementCanceled += OnPlayerMovementCanceled;
        PlayerInput.OnPlayerStartRunning += StartRunning;
        PlayerInput.OnPlayerStopRunning += StopRunning;
        PlayerSwitcher.OnPlayerChanged += UpdatePlayerInfo;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerMovementPerformed -= OnPlayerMovementPerformed;
        PlayerInput.OnPlayerMovementCanceled -= OnPlayerMovementCanceled;
        PlayerInput.OnPlayerStartRunning -= StartRunning;
        PlayerInput.OnPlayerStopRunning -= StopRunning;
        PlayerSwitcher.OnPlayerChanged -= UpdatePlayerInfo;
    }

    private void UpdatePlayerInfo(GameObject player, Player current)
    {
        IsRunning = false;
        Current = current;
    }

    bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            if (!Equals(_isRunning, value))
            {
                animController.SetBool("run", value);
                StartCoroutine(RampingSpeed(value ? runningSpeed : walkingSpeed));
                _isRunning = value;
            }
        }
    }

    public IEnumerator RampingSpeed(float target)
    {
        float count = 0;
        float start = playerSpeed;
        while (count < 1)
        {
            playerSpeed = Mathf.Lerp(start, target, count);
            count += Time.smoothDeltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    private void StartRunning()
    {
        IsRunning = true;
    }

    private void StopRunning()
    {
        IsRunning = false;
    }

    private void OnPlayerMovementPerformed(Vector2 direction)
    {
        if (!GlobalControl)
            return;
        if (Current != ControlFor)
            return;
        inputDirection = direction;
        animController.SetFloat("axis", 1);
    }

    public Transform PreviousCameraTransform;
    private void OnPlayerMovementCanceled()
    {
        if (!GlobalControl)
            return;
        if (Current != ControlFor)
            return;
        PreviousCameraTransform.position = Camera.main.transform.position;
        PreviousCameraTransform.rotation = Camera.main.transform.rotation;
        inputDirection = Vector2.zero;
        animController.SetFloat("axis", 0);
    }
}