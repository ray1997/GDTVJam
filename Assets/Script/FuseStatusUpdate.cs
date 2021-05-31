using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseStatusUpdate : MonoBehaviour
{
    public TMPro.TMP_Text Status;
    private void OnEnable()
    {
        if (!Objectives.Instance.IsQuestUnlock(GameManager.Instance.FuseQuestID))
        {
            Status.text = "Fuse busted";
            return;
        }
        else
        {
            if (!Objectives.Instance.IsQuestFinish(GameManager.Instance.FuseQuestID))
            {
                Status.text = "Need replacement fuse";
                return;
            }
            else
            {
                Status.text = "Fuse busted";
            }
            return;
        }        
    }
}
