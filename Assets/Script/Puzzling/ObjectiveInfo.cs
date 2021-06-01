using System.Collections.Generic;
using UnityEngine;

[System.Serializable()]
public class ObjectiveInfo
{
    public ObjectiveInfo() { }
    public ObjectiveInfo(string name) => Name = name;
    public ObjectiveInfo(string name, Player assigned) { Name = name; AssignedPlayer = assigned; }

    public string Name;
    public int ID;
    public Player AssignedPlayer;
    public TaskLocation Location;
    [SerializeField] bool _unlock;
    public bool IsUnlock
    {
        get => _unlock;
        set
        {
            _unlock = value; 
        }
    }

    public delegate void FinishObjective(ObjectiveInfo sender, int id);
    /// <summary>
    /// When quest finished
    /// </summary>
    public static event FinishObjective OnObjectiveFinished;

    [SerializeField] bool _done;
    public bool IsDone
    {
        get => _done;
        set
        {
            if (value)
            {
                Debug.Log($"Objective: [{ID}] {Name} completed");
                OnObjectiveFinished?.Invoke(this, ID);
            }
            _done = value;
        }
    }

    public List<int> Unlockable;
}
