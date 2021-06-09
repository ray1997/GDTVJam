using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DevTestTool : MonoBehaviour
{
    private void OnEnable()
    {
        Application.logMessageReceived += MessageReceived;
        PlayerInput.OnRequestEnterDebugFPS += OpenDebugConsole;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= MessageReceived;
        PlayerInput.OnRequestEnterDebugFPS -= OpenDebugConsole;
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
                "fps: Toggle between test fps mode and original camera view\r\n" +
                "unlock: Unlock all doors\r\n" +
                "skip: Skip all assigned quest on skipper\r\n" +
                "skip active: Skip currently active quests\r\n" +
                "skip [id]: Skip quest ID\r\n" +
                "randitem: Add random item to inventory\r\n";
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
