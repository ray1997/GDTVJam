using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public delegate void PlayerMovementPerformed(Vector2 direction);
    public static event PlayerMovementPerformed OnPlayerMovementPerformed;

    public delegate void PlayerMovementCanceled();
    public static event PlayerMovementCanceled OnPlayerMovementCanceled;

    public delegate void PlayerInteracted();
    public static event PlayerInteracted OnPlayerInteracted;

    public delegate void PlayerRequestSwitch();
    /// <summary>
    /// When player request to switch to other place;
    /// </summary>
    public static event PlayerRequestSwitch OnPlayerRequestSwitch;

    private MainInput inputManage;

    private void Awake()
    {
        inputManage = new MainInput();

        inputManage.Player.Movement.performed += context => { if (OnPlayerMovementPerformed != null) OnPlayerMovementPerformed(context.ReadValue<Vector2>()); };
        inputManage.Player.Movement.canceled += context => { if (OnPlayerMovementCanceled != null) OnPlayerMovementCanceled(); };
        inputManage.Player.Interact.performed += context => OnPlayerInteracted?.Invoke();
        inputManage.Player.SwitchRoom.performed += context => OnPlayerRequestSwitch?.Invoke();
    }

    private void OnEnable()
    {
        inputManage.Enable();
    }

    private void OnDisable()
    {
        inputManage.Disable();
    }
}