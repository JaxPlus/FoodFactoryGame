using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Quests/Quest")]
public class Quest : ScriptableObject
{
    public string questID;
    public string questName;
    public string description;
    public bool isUnlocked = false;
    public List<QuestObjective> objectives;
    public List<QuestReward> questRewards;

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(questID))
        {
            questID = questName + Guid.NewGuid().ToString();
        }
    }
}

[System.Serializable]
public class QuestObjective
{
    public string objectiveID; // musi byæ takie same jak to co chcemy zdobyæ np. przedmiot jakiœ
    public string description;
    //public ObjectiveType type;
    public int reqiredAmount;
    public int currentAmount;

    public bool IsCompleted => currentAmount >= reqiredAmount;
}

// Mo?e si? przyda lecz nie wiem
//public enum ObjectiveType
//{
//      Collectable, Item, Talk
//}

[System.Serializable]
public class QuestProgress
{
    public Quest quest;
    public List<QuestObjective> objectives;

    public QuestProgress(Quest quest)
    {
        this.quest = quest;
        objectives = new List<QuestObjective>();

        foreach (var obj in quest.objectives)
        {
            objectives.Add(new QuestObjective
            {
                objectiveID = obj.objectiveID,
                description = obj.description,
                reqiredAmount = obj.reqiredAmount,
                currentAmount = 0
            });
        }
    }
    public bool IsCompleted => objectives.TrueForAll(o => o.IsCompleted);
    public string QuestID => quest.questID;
}

[System.Serializable]
public class QuestReward
{
    public RewardType type;
    public int rewardID;
    public int amount = 1;
}

public enum RewardType { Item, Building, NextQuest, Reputation, Custom }