using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecurityCameras : MonoBehaviour
{
    public List<MeshRenderer> Screens;
    public List<GameObject> Cameras;
    public List<RenderTexture> Feeds;
    private void Start()
    {
        for (int i = 0; i < Screens.Count; i++)
        {
            Screens[i].materials[1].SetTexture("_MainTex", Feeds[i]);
        }
    }

    public Transform SecurityCamsView;
    public Transform SecurityRoomView;
    public bool UpdateCameraView;
    public void ActivateCameraFeed()
    {
        //Set camera view to feed view
        if (UpdateCameraView)
            Helper.help.MoveObject(Camera.main.transform, SecurityCamsView);
        foreach (var cam in Cameras)
        {
            cam.SetActive(true);
        }
    }

    public void DeactivateCameraFeed()
    {
        if (UpdateCameraView)
            Helper.help.MoveObject(Camera.main.transform, SecurityRoomView);
        foreach (var cam in Cameras)
        {
            cam.SetActive(false);
        }
    }
}
