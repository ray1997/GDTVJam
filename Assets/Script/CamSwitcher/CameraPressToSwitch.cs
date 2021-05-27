using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPressToSwitch : MonoBehaviour
{
    Transform PlayerTransform;
    Transform CameraTransform;
    private void Awake()
    {
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        CameraTransform = Camera.main.transform;
    }

    private void OnEnable()
    {
        PlayerInput.OnPlayerRequestSwitch += SwitchingCamera;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerRequestSwitch -= SwitchingCamera;
    }

    public bool Switchable;
    /// <summary>
    /// Targeting location on where to switch to.
    /// False => B | True => A
    /// </summary>
    public bool TargetAB;

    public Transform TargetPlayerPositionA;
    public Transform TargetCameraPositionA;

    public Transform TargetPlayerPositionB;
    public Transform TargetCameraPositionB;

    public Collider PlayerForcefield;

    public Animator ThatClip;
    private void SwitchingCamera()
    {
        if (!Switchable)
            return;
        //Temporary disable player
        PlayerTransform.gameObject.SetActive(false);
        //Temporary disable main camera
        CameraTransform.gameObject.SetActive(false);
        //Play animation overlay
        ThatClip.SetTrigger("OPEN");
        //Move player position based on Target boolean
        PlayerControl.ForceTriggerDisabler();
        MoveObject(PlayerTransform,
            TargetAB ? TargetPlayerPositionA : TargetPlayerPositionB);
        PlayerControl.ForceTriggerRestorer();
        //
        //Invoke camera move N seconds after animation finish?
        Invoke("MoveCameraAndShow", 3.25f);        
    }

    public void MoveCameraAndShow()
    {
        //Move camera position based on Target boolean
        MoveObject(CameraTransform,
            TargetAB ? TargetCameraPositionA : TargetCameraPositionB);
        //Toggle target AB
        TargetAB = !TargetAB;
        //Restore player
        PlayerTransform.gameObject.SetActive(true);
        //Restore camera
        CameraTransform.gameObject.SetActive(true);
    }

    public void MoveObject(Transform item, Transform target)
    {
        item.position = target.position;
        item.rotation = target.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        Switchable = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag != "Player") return;
        Switchable = false;
    }
}
