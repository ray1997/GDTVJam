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
    [SerializeField] bool _unlock;
    public bool IsUnlock
    {
        get => _unlock;
        set
        {
            if (value)
            {
                if (SubObjective?.Length > 0)
                {
                    foreach (var obj in SubObjective)
                    {
                        obj.IsUnlock = true;
                    }
                }
            }
            _unlock = value; 
        }
    }

    public delegate void FinishObjective(ObjectiveInfo sender, int id);
    public static event FinishObjective OnObjectiveFinished;

    [SerializeField] bool _done;
    public bool IsDone
    {
        get => _done;
        set
        {
            if (value)
            {
                if (!IsItAllDone())
                    value = false;
            }
            Debug.Log($"Objective: [{ID}] {Name} completed");
            _done = value;
            OnObjectiveFinished?.Invoke(this, ID);
        }
    }

    public bool IsItAllDone()
    {
        if (SubObjective is null)
            return true;
        if (SubObjective != null || SubObjective?.Length > 0)
        {
            //Check this first then set value
            foreach (ObjectiveInfo o in SubObjective)
            {
                if (!o.IsDone)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public ObjectiveInfo[] SubObjective;

    public int[] Unlockable;
}
