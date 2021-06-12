using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRandomItem : MonoBehaviour
{
    public List<InGameItem> Items;
    public void DoAddRandomItem()
    {
        PlayerState.RequestAddItem(Items[Random.Range(0, Items.Count)], PlayerSwitcher.Instance.CurrentPlayer);
    }
}
