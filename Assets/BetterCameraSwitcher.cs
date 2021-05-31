using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BetterCameraSwitcher : MonoBehaviour
{
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
        var point = CameraPoints.First(c => c.name == triggered.name);
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
