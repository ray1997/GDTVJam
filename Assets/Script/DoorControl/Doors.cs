using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Doors : MonoBehaviour
{
    public static Doors Instance;

    public Transform FirstFloorDoors;
    public Transform SecondFloorDoors;
    public List<DoorInfo> DoorsInfo;

    public void Awake()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);

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

    private void OnEnable()
    {
        ObjectiveInfo.OnObjectiveFinished += UnlockDoors;
        ItemPopulator.OnItemAttempUsage += UseItem;
    }

    private void UseItem(InGameItem info)
    {
        if (info.Unlock?.Count > 0)
        {
            Debug.Log("Try to unlock door with key");
            foreach (var unlockable in info.Unlock)
            {
                var door = DoorsInfo.FirstOrDefault(door => door.DoorName == unlockable);
                if (door is null)
                    continue;
                if (!DoorsInfo[DoorsInfo.IndexOf(door)].InteractableZone)
                    continue;
                DoorsInfo[DoorsInfo.IndexOf(door)].IsLocked = false;
                Debug.Log($"Unlock {door.DoorName} via an item");
            }
        }
    }

    private void UnlockDoors(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        switch (args.FinishedQuest)
        {
            case Quests.P1FindFlashlight: //Player 1 pickup flashlight
                UnlockDoors(DoorIdentity.WORKER1,
                    DoorIdentity.WORKER2,
                    DoorIdentity.WORKERB,
                    DoorIdentity.POWER,
                    DoorIdentity.HALL1A,
                    DoorIdentity.HALL1B);
                break;
            case Quests.P2FindFlashlight:
                UnlockDoors(DoorIdentity.BATH1, DoorIdentity.OFFICE);
                break;
            case Quests.P2OpenArtRoomDoor:
                UnlockDoors(DoorIdentity.ART, DoorIdentity.TANKA2, DoorIdentity.TANKB2, DoorIdentity.STORAGE);
                break;
            case Quests.P1DestroySecurityLock:
                UnlockDoors(DoorIdentity.SECURITY);
                break;
            case Quests.P2AxedHallwayDoor:
                UnlockDoors(DoorIdentity.STAIRWELLB2, DoorIdentity.GARAGE);
                break;
        }
    }

    public bool UnlockDoor(DoorIdentity name)
    {
        var door = DoorsInfo.IndexOf(DoorsInfo.FirstOrDefault(d => d.DoorName == name));
        if (door < 0)
            return false;
        DoorsInfo[door].IsLocked = false;
        return true;
    }

    public bool UnlockDoors(params DoorIdentity[] doors)
    {
        if (doors == null)
            return false;
        if (doors.Length < 1)
            return false;
        foreach (var door in doors)
        {
            UnlockDoor(door);
        }
        return true;
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
            tleft.TriggerEntering.AddListener(delegate { EnterInteractZone(t.name); });
            tleft.TriggerExiting.AddListener(delegate { CloseDoor(t.name); LeftInteractZone(t.name); });
            DoorsInfo.Add(new DoorInfo()
            {
                Door = t,
                DoorAnim = t.GetComponent<Animator>(),
                DoorIn = tin,
                DoorOut = tout,
                DoorLeft = tleft,
                DoorName = (DoorIdentity)System.Enum.Parse(typeof(DoorIdentity),t.name),
                IsLocked = true
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
            tleft.TriggerEntering.AddListener(delegate { EnterInteractZone(t.name); });
            tleft.TriggerExiting.AddListener(delegate { CloseDoor(t.name); LeftInteractZone(t.name); });
            DoorsInfo.Add(new DoorInfo()
            {
                Door = t,
                DoorAnim = t.GetComponent<Animator>(),
                DoorIn = tin,
                DoorOut = tout,
                DoorLeft = tleft,
                DoorName = (DoorIdentity)System.Enum.Parse(typeof(DoorIdentity), t.name),
                IsLocked = true
            });
        }
    }

    public void EnterInteractZone(string name)
    {
        var door = DoorsInfo.Find(d => d.DoorName.ToString() == name);
        if (door is null)
        {
            Debug.LogError("Unable to find a door " + name);
            return;
        }
        door.InteractableZone = true;
    }

    public void LeftInteractZone(string name)
    {
        var door = DoorsInfo.Find(d => d.DoorName.ToString() == name);
        if (door is null)
        {
            Debug.LogError("Unable to find a door " + name);
            return;
        }
        door.InteractableZone = false;
    }

    public List<LockReason> LockedInfo;

    public string GetLockInfo(string name)
    {
        var lockInfo = LockedInfo.FirstOrDefault(l => l.name == name);
        if (!(lockInfo is null))
        {
            return lockInfo.DoorLockedReason;
        }
        return "It's locked";
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

    [SerializeField] bool _interact;
    public bool InteractableZone
    {
        get => _interact;
        set
        {
            if (!Equals(_interact, value))
            {
                _interact = value;
                if (value && IsLocked)
                    ToastReceiver.ShowToastMessage(Doors.Instance.GetLockInfo(DoorName.ToString()));
            }
        }
    }

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