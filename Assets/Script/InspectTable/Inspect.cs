using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Inspect : MonoBehaviour
{
    private void Awake()
    {
        //Get player
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public bool AutoDetectTrigger;
    bool HaveTrigger;

    GameObject Player;
    private void OnEnable()
    {
        //Register interaction button
        PlayerInput.OnPlayerInteracted += AllowInteraction;
        PlayerSwitcher.OnPlayerChanged += UpdatePlayer;
    }
    DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> CameraPos;
    DG.Tweening.Core.TweenerCore<Quaternion, Quaternion, DG.Tweening.Plugins.Options.NoOptions> CameraRot;
    private void UpdatePlayer(GameObject player)
    {
        Player = player;
    }

    private void OnDisable()
    {
        PlayerInput.OnPlayerInteracted -= AllowInteraction;
        PlayerSwitcher.OnPlayerChanged -= UpdatePlayer;
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
    public Transform RestorePosition;

    public string InspectID = "";

    public bool InTableView;
    public List<GameObject> InspectableItems = new List<GameObject>();
    public void InspectTable()
    {
        Debug.Log("Entering inspect table");
        //Show interactable items
        foreach (var btn in InspectableItems)
        {
            if (btn is null)
                continue;
            btn.SetActive(true);
        }
        //Disable player control
        PlayerControl.ForceTriggerDisabler();
        //Lerp camera to that inspect position
        Camera.main.transform.DOMove(InspectCameraPosition.transform.position, MovingTime);
        Camera.main.transform.DORotateQuaternion(InspectCameraPosition.transform.rotation, MovingTime);
        DOTween.Play(Camera.main.transform);
        //StartCoroutine(MovingCamera(Camera.main.transform, InspectCameraPosition.transform));

        OnBeganInspecting?.Invoke(InspectID);
        InTableView = true;
    }

    public void LeaveTable()
    {
        Debug.Log("Leaving inspect table");
        //Hide interactable items
        foreach (var btn in InspectableItems)
        {
            if (btn is null)
                continue;
            btn.SetActive(false);
        }
        //Restore camera position
        Camera.main.transform.DOMove(RestorePosition.position, MovingTime);
        Camera.main.transform.DORotateQuaternion(RestorePosition.rotation, MovingTime);
        DOTween.Play(Camera.main.transform);
        //StartCoroutine(MovingCamera(Camera.main.transform, CachedLocation));
        //Restore player
        PlayerControl.ForceTriggerRestorer();
        OnStopInspecting?.Invoke(InspectID);
        InTableView = false;
    }

    [Range(0.5f, 3)]
    public float MovingTime = 1;

    Transform movingFrom;
    Transform movingTo;
    IEnumerator MovingCamera(Transform item, Transform target)
    {
        movingFrom = item;
        movingTo = target;
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
        movingFrom = movingTo = null;
        yield return new WaitForFixedUpdate();
    }
    
    bool _interactable;
    public bool Interactable
    {
        get => _interactable;
        set => _interactable = value;
    }
    
}
