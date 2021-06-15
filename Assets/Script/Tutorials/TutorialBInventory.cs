using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TutorialBInventory : MonoBehaviour
{
    public PlayerState Player1Inv;

    public void DelayDisable() => TutorialRoot.SetActive(false);
    public GameObject TutorialRoot;

    private void OnTriggerEnter(Collider other)
    {
        //Check inventory for context
        var fuse = Player1Inv.PlayerInventory.FirstOrDefault(i => i.Name == "Fuse");
        if (fuse is null)
        {
            if (Objectives.Instance.IsQuestFinish((int)Quests.P1ReplaceFuse))
                ToastReceiver.ShowToastMessage("Fuse replaced, ready to switch on!");
            else
                ToastReceiver.ShowToastMessage("It's appear that the fuse is busted.\r\nGo find a replacement fuse");
        }
        else
            ToastReceiver.ShowToastMessage("You found a fuse!\r\nPress [Q] to open inventory\r\nThen use fuse it to replace the broken one");
    }
}
