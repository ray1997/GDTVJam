using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable()]
public class ObjectiveInfo : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    public void Notify(string propertyName) => PropertyChanged?.Invoke(this, 
        new PropertyChangedEventArgs(propertyName));

    public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
    {
        if (!Equals(storage, value))
        {
            storage = value;
            Notify(propertyName);
            return true;
        }
        return false;
    }

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
        set => Set(ref _unlock, value);
    }

    public delegate void FinishObjective(ObjectiveInfo sender, ObjectiveFinishedEventArgs args);
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
            if (Set(ref _done, value))
            {
                Debug.Log($"Objective: [{ID}] {Name} completed");
                OnObjectiveFinished?.Invoke(this, new ObjectiveFinishedEventArgs(ID));
            }
        }
    }

    public List<int> Unlockable;
}

public class ObjectiveFinishedEventArgs : System.EventArgs
{
    public Quests FinishedQuest { get; private set; }
    public ObjectiveFinishedEventArgs(int questID)
    {
        FinishedQuest = (Quests)questID;
    }
}
