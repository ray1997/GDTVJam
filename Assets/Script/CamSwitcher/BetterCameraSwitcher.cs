using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BetterCameraSwitcher : MonoBehaviour
{
    public delegate void ToggleCameraUpdator();
    public static event ToggleCameraUpdator OnRequestStopCameraUpdate;

    public static void ForceToggleCameraUpdate() => OnRequestStopCameraUpdate?.Invoke();

    private void Start()
    {
        OnRequestStopCameraUpdate += ToggleCameraUpdate;
    }

    private void ToggleCameraUpdate()
    {
        if (untilSwitchAgain == float.MaxValue)
            untilSwitchAgain = Time.realtimeSinceStartup + SwitchCooldown;
        else
            untilSwitchAgain = float.MaxValue;
    }

    public Camera MainCamera;

    public List<GameObject> CameraPoints;

    float untilSwitchAgain;
    public float SwitchCooldown = 2;

    public Collider CurrentlyStayed;
    public void RequestSwitchTo(Collider triggered)
    {
        CurrentlyStayed = triggered;
        //Cooldown
        if (Time.realtimeSinceStartup < untilSwitchAgain)
            return;
        untilSwitchAgain = Time.realtimeSinceStartup + SwitchCooldown;
        //Switch to requested room
        //Get camera point
        var point = CameraPoints.FirstOrDefault(c => c.name == triggered.name);
        if (point is null)
        {
            Debug.LogError($"Error: No camera point with a name: {triggered.name}");
            return;
        }
        //Switch camera to that position
        MainCamera.transform.position = point.transform.position;
        MainCamera.transform.rotation = point.transform.rotation;
    }
}
