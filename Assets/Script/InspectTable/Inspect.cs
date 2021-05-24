using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspect : MonoBehaviour
{
    GameObject Player;
    private void OnEnable()
    {
        //Register interaction button
        PlayerInput.OnPlayerInteracted += AllowInteraction;
        //Get player
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerInteracted -= AllowInteraction;
    }
    private void AllowInteraction()
    {
        if (!Interactable)
            return;
        if (!InTableView)
            InspectTable();
        else
            LeaveTable();
    }

    public delegate void BeginInspect(string itemID);
    public static event BeginInspect OnBeganInspecting;

    public delegate void StopInspect(string itemID);
    public static event StopInspect OnStopInspecting;

    public GameObject InspectCameraPosition;
    public Transform OriginalCamPosition;

    public string InspectID = "";

    public bool InTableView;
    public void InspectTable()
    {
        StopAllCoroutines();
        //Remove all highlight
        //Gather all trigger highlight
        TriggerHighlight[] highlights = FindObjectsOfType<TriggerHighlight>();
        foreach (var h in highlights)
        {
            h.RestoreMaterials();
        }
        //Hide player
        Player.SetActive(false);
        //Lerp camera to that inspect position
        StartCoroutine(MovingCamera(Camera.main.transform, InspectCameraPosition.transform));

        OnBeganInspecting?.Invoke(InspectID);
        InTableView = true;
    }

    public void LeaveTable()
    {
        StopAllCoroutines();
        //Restore camera position
        StartCoroutine(MovingCamera(Camera.main.transform, OriginalCamPosition));
        //Restore player
        Player.SetActive(true);
        OnStopInspecting?.Invoke(InspectID);
        InTableView = false;
    }

    [Range(0.5f, 3)]
    public float MovingTime = 1;
    IEnumerator MovingCamera(Transform item, Transform target)
    {
        float moving = 0;
        Vector3 cachedBeginP = item.position;
        Quaternion cachedBeginR = item.rotation;
        while (moving < MovingTime)
        {
            item.position = Vector3.Lerp(cachedBeginP, target.position, (moving / MovingTime));
            item.rotation = Quaternion.Lerp(cachedBeginR, target.rotation, (moving / MovingTime));
            moving += Time.smoothDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        //Incase of extra -seconds
        item.position = target.position;
        item.rotation = target.rotation;
        yield return new WaitForFixedUpdate();
    }

    bool _interactable;
    public bool Interactable
    {
        get => _interactable;
        set => _interactable = value;
    }
    
}
