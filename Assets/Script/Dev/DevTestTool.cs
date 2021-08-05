using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevTestTool : MonoBehaviour
{
    private void OnEnable()
    {
        Application.logMessageReceived += MessageReceived;
        PlayerInput.OnRequestToggleConsole += OpenDebugConsole;
        if (Application.isEditor)
            Invoke(nameof(DelayDoorsUnlock), 2f);
    }

    private void DelayDoorsUnlock()
    {
        DoorManager.DoorsInfo.ForEach(d => d.IsLocked = false);
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= MessageReceived;
        PlayerInput.OnRequestToggleConsole -= OpenDebugConsole;
    }

    public bool IsShowingConsole;
    public Transform LogConsole;
    public TMP_InputField TextInput;
    private void OpenDebugConsole()
    {
        IsShowingConsole = !IsShowingConsole;
        LogConsole.transform.DOLocalMove(
            IsShowingConsole ? Vector3.zero : new Vector3(0, (Screen.height * 5)), 1);
        TextInput.interactable = IsShowingConsole;
        if (IsShowingConsole)
            TextInput.Select();
    }

    public TMP_Text LogDisplay;
    private void MessageReceived(string condition, string stackTrace, LogType type)
    {
        LogDisplay.text += $"<color=#{GetColorCode(type)}>{condition}</color>\r\n";
    }

    public string GetColorCode(LogType type)
    {
        switch (type)
        {
            case LogType.Log:
                return ColorUtility.ToHtmlStringRGBA(Color.white);
            case LogType.Warning:
                return ColorUtility.ToHtmlStringRGBA(Color.yellow);
            case LogType.Error:
                return ColorUtility.ToHtmlStringRGBA(Color.red);
            default:
                return ColorUtility.ToHtmlStringRGBA(Color.white);
        }
    }

    public DisableMainCam FPSSwitcher;
    public QuestSkip Skipper;
    public Doors DoorManager;
    public AddRandomItem Items;
    public PlayerControl[] Players;
    public void SubmitCommand()
    {
        string cmd = TextInput.text;
        TextInput.text = "";
        LogDisplay.text += $"{cmd}\r\n";
        if (cmd.StartsWith("help"))
        {
            //List all commands
            LogDisplay.text += "Available commands:\r\n" +
                "help: Show all commands\r\n" +
#if UNITY_EDITOR
                "dev: Run a few specific commands\r\n" +
#endif
                "fps: Toggle between test fps mode and original camera view\r\n" +
                "unlock: Unlock all doors\r\n" +
                "skip: Skip all assigned quest on skipper\r\n" +
                "skip active: Skip currently active quests\r\n" +
                "skip [id]: Skip quest ID\r\n" +
                "randitem: Add random item to inventory\r\n" +
                "lift [second]: Change elevator travel time\r\n" +
                "walk [speed]: Change both player walk speed\r\n" +
                "exit: Exit the game";
        }
        else if (cmd.StartsWith("exit"))
        {
            Application.Quit();
        }
        else if (cmd.StartsWith("reset"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
#if UNITY_EDITOR
        else if (cmd.StartsWith("dev"))
        {
            //Set player speed
            Players[0].walkingSpeed = 4;
            Players[0].playerSpeed = Players[0].playerSpeed == 0 ? 0 : Players[0].walkingSpeed;
            Players[1].walkingSpeed = 4;
            Players[1].playerSpeed = Players[1].playerSpeed == 0 ? 0 : Players[1].walkingSpeed;
            //Set elevator speed
            ElevatorControl.Instance.ElevatorTravelTime = 1;
            //Skip quests
            Skipper.SkipQuest();
        }
#endif
        else if (cmd.StartsWith("walk"))
        {
            var param = cmd.Substring("walk".Length).Trim();
            float.TryParse(param, out float speed);
            if (speed != float.NaN)
            {
                LogDisplay.text += $"Set both player walk speed to: {speed}\r\n";
                Players[0].walkingSpeed = speed;
                Players[0].playerSpeed = Players[0].playerSpeed == 0 ? 0 : Players[0].walkingSpeed;
                Players[1].walkingSpeed = speed;
                Players[1].playerSpeed = Players[1].playerSpeed == 0 ? 0 : Players[1].walkingSpeed;
            }
        }
        else if (cmd.StartsWith("lift"))
        {
            var param = cmd.Substring("lift".Length).Trim();
            float.TryParse(param, out float sec);
            if (sec != float.NaN)
            {
                LogDisplay.text += $"Set elevator travel time to: {sec}\r\n";
                ElevatorControl.Instance.ElevatorTravelTime = sec;
            }
        }
        else if (cmd.StartsWith("fps"))
        {
            LogDisplay.text += "Toggle FPS\r\n";
            FPSSwitcher.EnableDebugView();
        }
        else if (cmd.StartsWith("unlock"))
        {
            LogDisplay.text += "Doors unlocked\r\n";
            DoorManager.DoorsInfo.ForEach(d => d.IsLocked = false);
        }
        else if (cmd.StartsWith("skip"))
        {
            if (cmd == "skip")
            {
                LogDisplay.text += $"Skipping skipper assigned quests";
                Skipper.SkipQuest();
            }
            else if (cmd == "skip active")
            {
                Objectives.Instance.ActiveObjectives.Where(q => q.IsUnlock).ToList().ForEach(
                    q =>
                    {
                        Objectives.Instance.MarkQuestAsFinish(q.ID);
                    });
            }
            else
            {
                if (!cmd.Contains(","))
                {
                    var qid = cmd.Substring(5).Trim();
                    int.TryParse(qid, out int idParsed);
                    Objectives.Instance.MarkQuestAsFinish(idParsed);
                }
                else if (cmd.Contains(","))
                {
                    var qids = cmd.Substring(5).Trim().Split(',');
                    foreach (var qid in qids)
                    {
                        int.TryParse(qid, out int tryParsed);
                        Objectives.Instance.MarkQuestAsFinish(tryParsed);
                    }
                }
            }
        }
        else if (cmd.StartsWith("randitem"))
        {
            LogDisplay.text += $"Adding item";
            Items.DoAddRandomItem();
        }
    }
}
