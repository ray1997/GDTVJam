using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class Objectives : MonoBehaviour
{
    public List<ObjectiveInfo> ActiveObjectives;
    public TextAsset QuestBase;

    private void Awake()
    {
        ActiveObjectives = new List<ObjectiveInfo>();
        var infos = QuestBase.text.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        foreach (var info in infos)
        {
            if (info.StartsWith("ID"))
                continue;
            var sub = info.Split(',');
            ActiveObjectives.Add(new ObjectiveInfo()
            {
                ID = int.Parse(sub[0]),
                Unlockable = ParseUnlockable(sub[1]),
                Name = sub[3],
                AssignedPlayer = sub[4] == "Player1" ? Player.First : Player.Second,
                Location = ParseEnum<TaskLocation>(sub[5]),
                IsUnlock = sub[6] == "1",
                IsDone = sub[7] == "1"
            });
        }
    }

    public T ParseEnum<T>(string input) => (T)Enum.Parse(typeof(T), input);

    public List<int> ParseUnlockable(string input)
    {
        if (input == "0")
            return new List<int>();
        if (input.Contains(":"))
        {
            return input.Split(':')
                .Select(i => int.Parse(i))
                .ToList();
        }
        else
        {
            return new List<int>()
            {
                int.Parse(input)
            };
        }
    }

    public static Objectives Instance;

    private void Start()
    {
        if (Instance is null)
            Instance = this;
        else
            Destroy(gameObject);
        PlayerState.OnRequestAddingItem += InventoryCheck;
        ObjectiveInfo.OnObjectiveFinished += UnlockableCheck;
    }

    private void UnlockableCheck(ObjectiveInfo sender, ObjectiveFinishedEventArgs args)
    {
        if (sender.Unlockable?.Count > 0)
        {
            foreach (var quest in ActiveObjectives)
            {
                if (sender.Unlockable.Contains(quest.ID))
                {
                    quest.IsUnlock = true;
                    Debug.Log($"Try to mark quest as finish found {quest.ID} and now marked as finished");
                    return;
                }
            }
            Debug.Log($"Unable to find quest with the ID {(int)args.FinishedQuest}");
        }
    }

    private void InventoryCheck(InGameItem info, Player Target = Player.First)
    {
        if (info.ItemSepecification == Specific.UnlockFlashlight)
        {

        }
    }

    public bool IsQuestFinish(int id)
    {
        bool haveQuest = ActiveObjectives.Any(q => q.ID == id);
        if (!haveQuest)
            return false;
        return ActiveObjectives.FirstOrDefault(q => q.ID == id).IsDone;
    }

    public bool IsQuestUnlock(int id)
    {
        bool haveQuest = ActiveObjectives.Any(q => q.ID == id);
        if (!haveQuest)
            return false;
        return ActiveObjectives.FirstOrDefault(q => q.ID == id).IsUnlock;
    }

    public void MarkQuestAsFinish(int id)
    {
        Debug.Log("Try to mark quest ID " + id + "as finished");
        var quest = ActiveObjectives.IndexOf(ActiveObjectives.FirstOrDefault(q => q.ID == id));
        Debug.Log($"Attempt {(quest == -1 ? "failed" : "working, found quest with that ID")}");
        if (quest >= 0)
        {
            ActiveObjectives[quest].IsDone = true;
        }
    }

    public void MarkQuestAsFinish(Quests id) => MarkQuestAsFinish((int)id);
}

public enum Quests
{
    P1FindFlashlight = 203,
    P1FindFuse = 301,
    P1ReplaceFuse = 399,
    P1TurnOnCircuit = 375,
    P2FindFlashlight = 208,
    P2FindElevatorSwitch = 209,
    P2PutSwitchToElevator = 117,
    P2ActivateElevator = 136,
    P1FindBoltCutter = 305,
    P1SendBoltCutter2P2 = 411,
    P2GetBoltCutterFromP1 = 361,
    P2OpenArtRoomDoor = 176,
    P2FindChemistryBook = 144,
    P2SendChemistryBookToP1 = 347,
    P1FindAPenAndPaper = 51,
    P1WriteDownChemListToP2 = 474,
    P1FindChemForLockMelt1 = 179,
    P1FindChemForLockMelt2 = 420,
    P1FindChemForLockMelt3 = 249,
    P2FindChemForLockMelt1 = 485,
    P2FindChemForLockMelt2 = 185,
    P2FindChemForLockMelt3 = 115,
    P2SendChemicalsToP1 = 153,
    P1MixLockMelter = 325,
    P1DestroySecurityLock = 64,
    P1GetAxe = 90,
    P2TakeAxe = 110,
    P2AxedHallwayDoor = 2,
    P2InspectCar = 273,
    P2GiveListOfWishlistToP1 = 455,
    P1TakeWishlistFromP2 = 75,
    P1FindEquipmentToFixCar1 = 95,
    P1FindEquipmentToFixCar2 = 326,
    P2FindEquipmentToFixCar3 = 439,
    P1GetACarKey = 412,
    P2FixCar = 389,
    P1GetOut = 228,
    P2GetOut = 303
}