using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Ingame/Item", order = 1)]
public class InGameItem : ScriptableObject
{
    public string Name;
    public Specific ItemSepecification;
    public int ForQuestID;
}