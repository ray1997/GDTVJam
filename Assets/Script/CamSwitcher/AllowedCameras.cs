using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllowedCameras : MonoBehaviour
{
    private void OnEnable()
    {
        if (SwitchableCameras is null || SwitchableCameras.Count < 1)
        {
            foreach (Transform t in transform)
            {
                SwitchableCameras.Add(t.gameObject);
            }
        }
        PlayerSwitcher.OnPlayerChanged += PlayerChanged;
        BetterCameraSwitcher.OnPlayerChangingRoom += UpdateAllowedCameras;
    }

    private void UpdateAllowedCameras(Player current, Collider c)
    {
        if (CurrentActivePlayer != current)
        {
            return;
        }
        var rule = CameraRules.FirstOrDefault(cu => cu.name == c.name);
        if (c.transform.parent.name.StartsWith("First"))
        {
            //Disable all second floor cameras
            foreach (var camera in SwitchableCameras)
            {
                if (rule != null)
                {
                    camera.SetActive(ShouldActivateBasedOnExistingRule(camera, rule));
                    //Skip the rest of checking if it has a rule and state not wanting to continue checking
                    if (!rule.ShouldStillUseFloorCheck)
                        continue;
                }

                //Per floor camera (For rule less room)
                if (camera.name.StartsWith("S"))
                {
                    camera.SetActive(false);
                }
                else
                {
                    camera.SetActive(true);
                }

            }
        }
        else if (c.transform.parent.name.StartsWith("Second"))
        {
            foreach (var camera in SwitchableCameras)
            {
                if (rule != null)
                {
                    camera.SetActive(ShouldActivateBasedOnExistingRule(camera, rule));
                    //Skip the rest of checking if it has a rule and state not wanting to continue checking
                    if (!rule.ShouldStillUseFloorCheck)
                        continue;
                }

                //Per floor camera (For rule less room)
                if (camera.name.StartsWith("S"))
                {
                    camera.SetActive(false);
                }
                else
                {
                    camera.SetActive(true);
                }
            }
        }
    }

    Player CurrentActivePlayer = Player.First;
    private void PlayerChanged(GameObject player, Player current)
    {
        CurrentActivePlayer = current;
    }

    public List<GameObject> SwitchableCameras;
    public List<CameraActivateRules> CameraRules;

    /// <summary>
    /// Should camera be activate based on attached rule
    /// </summary>
    /// <param name="camera">Camera that wanted to check</param>
    /// <param name="rule">That specific camera rule</param>
    /// <returns></returns>
    private bool ShouldActivateBasedOnExistingRule(GameObject camera, CameraActivateRules rule)
    {
        if (rule.ListMode == CameraListRuleMode.Blacklist)
        {
            //If rule for this location exist and is a blacklist;
            //Enable all cameras and we'll turn it off based on list
            if (rule.CameraList.Contains(camera.name))
            {
                //This camera is on blacklist?
                //Disable it no matter what floor it was!
                return false;
            }
            return true;
        }
        else if (rule.ListMode == CameraListRuleMode.Whitelist)
        {
            //If rule for this location exist and is a whitelist;
            //Disable all cameras and we'll turn it on based on list
            if (rule.CameraList.Contains(camera.name))
            {
                //This camera is on whitelist?
                //Enable it no matter what floor it was!
                return true;
            }
        }
        return false;
    }
}
