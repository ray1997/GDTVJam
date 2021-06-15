using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LockInfo", menuName = "Ingame/LockInfo", order = 3)]
public class LockReason : ScriptableObject
{
    public string DoorLockedReason;
}
