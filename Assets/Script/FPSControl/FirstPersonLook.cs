using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    Vector2 currentMouseLook;
    Vector2 appliedMouseDelta;
    public float sensitivity = 1;
    public float smoothing = 2;


    void Reset()
    {
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    private void OnEnable()
    {
        PlayerInput.OnCameraMovementPerformed += MovingCamera;
        PlayerInput.OnCameraMovementCancelled += StopCamera;
    }

    Vector2 movingDelta;
    private void StopCamera()
    {
        movingDelta = Vector2.zero;
    }

    private void MovingCamera(Vector2 delta)
    {
        movingDelta = delta;
    }

    void Update()
    {
        // Get smooth mouse look.
        Vector2 smoothMouseDelta = Vector2.Scale(movingDelta, Vector2.one * sensitivity * smoothing);
        appliedMouseDelta = Vector2.Lerp(appliedMouseDelta, smoothMouseDelta, 1 / smoothing);
        currentMouseLook += appliedMouseDelta;
        currentMouseLook.y = Mathf.Clamp(currentMouseLook.y, -90, 90);

        // Rotate camera and controller.
        transform.localRotation = Quaternion.AngleAxis(-currentMouseLook.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(currentMouseLook.x, Vector3.up);
    }
}
