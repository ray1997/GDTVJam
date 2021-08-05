using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rule", menuName = "Ingame/Camera rules", order = 1)]
public class CameraActivateRules : ScriptableObject
{
    public CameraListRuleMode ListMode;
    public List<string> CameraList;

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