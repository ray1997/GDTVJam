using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public Transform FirstFloorDoors;
    public Transform SecondFloorDoors;
    public List<DoorInfo> DoorsInfo;

    public void Awake()
    {
        DoorsInfo = new List<DoorInfo>();
        foreach (Transform t in FirstFloorDoors)
        {
            AddDoor(t);
        }
        foreach (Transform t in SecondFloorDoors)
        {
            AddDoor(t);
        }
    }

    public void AddDoor(Transform t)
    {
        if (t.name == "EXIT")
            return;
        Transform big = t.transform.Find("BigDoorTriggers");
        Transform small = t.transform.Find("SmallDoorTriggers");
        if (!big && !small)
        {
            Debug.LogError($"Error! Can't find trigger for both big or small door on door {t.name}");
            return;
        }
        if (big != null)
        {
            var tin = big.Find("In").gameObject.AddComponent<TriggerEvents>();
            tin.Initialize();
            tin.TriggerEntering.AddListener(delegate { OpenInward(t.name); });
            var tout = big.Find("Out").gameObject.AddComponent<TriggerEvents>();
            tout.Initialize();
            tout.TriggerEntering.AddListener(delegate { OpenOutward(t.name); });
            var tleft = big.Find("Left").gameObject.AddComponent<TriggerEvents>();
            tleft.Initialize();
            tleft.TriggerExiting.AddListener(delegate { CloseDoor(t.name); });
            DoorsInfo.Add(new DoorInfo()
            {
                Door = t,
                DoorAnim = t.GetComponent<Animator>(),
                DoorIn = tin,
                DoorOut = tout,
                DoorLeft = tleft,
                DoorName = (DoorIdentity)System.Enum.Parse(typeof(DoorIdentity),t.name),
                IsLocked = false
            });
        }
        if (small != null)
        {
            var tin = small.Find("In").gameObject.AddComponent<TriggerEvents>();
            tin.Initialize();
            tin.TriggerEntering.AddListener(delegate { OpenInward(t.name); });
            var tout = small.Find("Out").gameObject.AddComponent<TriggerEvents>();
            tout.Initialize();
            tout.TriggerEntering.AddListener(delegate { OpenOutward(t.name); });
            var tleft = small.Find("Left").gameObject.AddComponent<TriggerEvents>();
            tleft.Initialize();
            tleft.TriggerExiting.AddListener(delegate { CloseDoor(t.name); });
            DoorsInfo.Add(new DoorInfo()
            {
                Door = t,
                DoorAnim = t.GetComponent<Animator>(),
                DoorIn = tin,
                DoorOut = tout,
                DoorLeft = tleft,
                DoorName = (DoorIdentity)System.Enum.Parse(typeof(DoorIdentity), t.name),
                IsLocked = false
            });
        }
    }

    public void OpenInward(string name)
    {
        var door = DoorsInfo.Find(d => d.DoorName.ToString() == name);
        if (door is null)
        {
            Debug.LogError("Unable to find a door " + name);
            return;
        }
        if (door.IsLocked)
        {
            Debug.LogWarning($"Can't open door {name} it's locked");
            return;
        }
        if (door != null)
        {
            door.DoorAnim.SetBool("Inward", true);
            door.DoorAnim.SetBool("Open", true);
        }
    }

    public void OpenOutward(string name)
    {
        var door = DoorsInfo.Find(d => d.DoorName.ToString() == name);
        if (door is null)
        {
            Debug.LogError("Unable to find a door " + name);
            return;
        }

        if (door.IsLocked)
        {
            Debug.LogWarning($"Can't open door {name} it's locked");
            return;
        }
        if (door != null)
        {
            door.DoorAnim.SetBool("Inward", false);
            door.DoorAnim.SetBool("Open", true);
        }
    }

    public void CloseDoor(string name)
    {
        var door = DoorsInfo.Find(d => d.DoorName.ToString() == name);
        if (door is null)
        {
            Debug.LogError("Unable to find a door " + name);
            return;
        }
        door.DoorAnim.SetBool("Open", false);
    }
}

[System.Serializable()]
public class DoorInfo
{
    public DoorIdentity DoorName;
    public bool IsLocked;
    public Transform Door;
    public Animator DoorAnim;
    public TriggerEvents DoorIn;
    public TriggerEvents DoorOut;
    public TriggerEvents DoorLeft;

    public DoorInfo()
    {

    }
}

public enum DoorIdentity
{
    HALL1B,
    WORKER1,
    WORKER2,
    POWER,
    WORKERB,
    HALL1A,
    TANKB1,
    STAIRWELLB1,
    TANKA1,
    SECURITY,
    LOUNGE,
    KITCHEN,
    DINING,
    STAIRWELLA1,
    GARAGE,
    BED2,
    STAIRWELLB2,
    STAIRWELLA2,
    TANKB2,
    BED1,
    BATH1,
    TANKA2,
    LIBRARY,
    OFFICE,
    STORAGE,
    ART,
    GAME,
    BATH2
}