using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllowedCameras : MonoBehaviour
{
    private void Start()
    {
        Player1 = GameObject.Find("Player1").transform;
        Player2 = GameObject.Find("Player2").transform;
    }

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
        CameraActivateRules rule = null;
        rule = CameraRules.FirstOrDefault(cu => cu.RoomCollider.name == c.name);
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

                //Per floor camera (For ruleless room)
                if (camera.name.StartsWith("S"))
                {
                    camera.SetActive(false);
                }
                else
                {
                    if (rule == null)
                        camera.SetActive(true);
                }

                //Update camera based on distance
                ForceUpdateDistance = true;
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

                //Per floor camera (For ruleless room)
                if (camera.name.StartsWith("F"))
                {
                    camera.SetActive(false);
                }
                else
                {
                    if (rule == null)
                        camera.SetActive(true);
                }

                //Update camera based on distance
                ForceUpdateDistance = true;
            }
        }
    }


    bool ForceUpdateDistance;
    public float ConsiderationDistance;
    //private void Update()
    //{
    //    if (Time.frameCount % 30 == 0 || ForceUpdateDistance)
    //    {
    //        if (SwitchableCameras is null
    //            || Player1 is null
    //            || Player2 is null)
    //            return;
    //        Debug.Log("Disabling cameras that are too far!");
    //        Update every 30 frame
    //        Camera activate field
    //        foreach (var camera in SwitchableCameras)
    //        {
    //            Check if current player too far from this camera or not
    //            if (Vector3.Distance(camera.transform.position,
    //                CurrentActivePlayer == Player.First ? Player1.position : Player2.position)
    //                > ConsiderationDistance)
    //            {
    //                If too far; Disable it
    //                camera.SetActive(false);
    //            }
    //        }
    //        disable force update
    //        if (ForceUpdateDistance)
    //            ForceUpdateDistance = false;
    //    }
    //}

    Transform Player1;
    Transform Player2;

    Player CurrentActivePlayer = Player.First;
    private void PlayerChanged(GameObject player, Player current)
    {
        if (current == Player.First)
            Player1 = player.transform;
        else
            Player2 = player.transform;

        CurrentActivePlayer = current;
        //Force update allowed cameras
        UpdateAllowedCameras(CurrentActivePlayer,
            CurrentActivePlayer == Player.First ?
            BetterCameraSwitcher.Instance.CurrentlyStayed1 :
            BetterCameraSwitcher.Instance.CurrentlyStayed2);
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
            if (rule.CameraList.FirstOrDefault(r => r.name == camera.name) != null)
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
            if (rule.CameraList.FirstOrDefault(r => r.name == camera.name) != null)
            {
                //This camera is on whitelist?
                //Enable it no matter what floor it was!
                return true;
            }
        }
        return false;
    }
}
