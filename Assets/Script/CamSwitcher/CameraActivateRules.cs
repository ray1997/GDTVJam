using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable()]
public class CameraActivateRules
{
    public Collider RoomCollider;
    public CameraListRuleMode ListMode;
    public List<GameObject> CameraList;

    /// <summary>
    /// Should rule still let it checking for floor differentiation;
    /// otherise, use rule list only
    /// </summary>
    public bool ShouldStillUseFloorCheck;
}

public enum CameraListRuleMode
{
    Whitelist,
    Blacklist
}