using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TaskPopulator : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Button TaskButton;
    public Image TaskBackgrond;
    public TMP_Text TaskLocation;
    public TMP_Text TaskName;
    public Quests TaskID;

    public void Initialize(ObjectiveInfo task)
    {
        TaskID = (Quests)task.ID;
        //Location
        TaskLocation.text = TranslateComputerEnumToHumananeLocationName(task.Location);
        TaskName.text = task.Name;
        //Register to task and destroy itself when task finish
        ObjectiveInfo.OnObjectiveFinished += WhenFinished;
    }

    private void WhenFinished(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (TaskID != args.FinishedQuest)
            return;
        ObjectiveInfo.OnObjectiveFinished -= WhenFinished;
        Destroy(gameObject);
    }

    public string TranslateComputerEnumToHumananeLocationName(TaskLocation location)
    {
        switch (location)
        {
            case global::TaskLocation.Power:
                return "Power storage";
            case global::TaskLocation.WorkerBathroom:
                return "Worker's bathroom";
            case global::TaskLocation.Worker1:
                return "Worker's bedroom 1";
            case global::TaskLocation.Worker2:
                return "Worker's bedroom 2";
            case global::TaskLocation.Hall1ST:
            case global::TaskLocation.Hall1ST2:
            case global::TaskLocation.Hall1STEnd:
                return "Hallway 1st floor";
            case global::TaskLocation.Hall2ND:
                return "Hallway 2nd floor";
            case global::TaskLocation.Stairwell1:
            case global::TaskLocation.Stairwell2:
                return "Stairwell";
            case global::TaskLocation.FishtankBottom:
            case global::TaskLocation.FishtankTop:
                return "Fish tank";
            case global::TaskLocation.Bedroom1:
                return "Bedroom 1";
            case global::TaskLocation.Bedroom2:
                return "Bedroom 2";
            case global::TaskLocation.Bathroom1:
                return "Bathroom 1";
            case global::TaskLocation.Bathroom2:
                return "Bathroom 2";
            default:
                return location.ToString();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TaskBackgrond.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TaskBackgrond.gameObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        TaskBackgrond.gameObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        TaskBackgrond.gameObject.SetActive(false);
    }
}

public enum TaskLocation
{
    Power,
    WorkerBathroom,
    Worker1,
    Worker2,
    Hall1ST,
    Hall1ST2,
    Hall1STEnd,
    Kitchen,
    Dining,
    Stairwell1,
    Garage,
    FishtankBottom,
    Exit,
    Lounge,
    Security,
    Bathroom1,
    Bedroom1,
    Office,
    Art,
    Stairwell2,
    Bedroom2,
    Bathroom2,
    Storage,
    Gameroom,
    FishtankTop,
    Hall2ND,
    Library
}