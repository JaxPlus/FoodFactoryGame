using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Instance { get; private set; }
    public List<Quest> allQuests = new();
    public List<QuestProgress> activeQuests = new();
    private QuestUI questUI;
    
    public List<string> handingQuestIDs = new();
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        questUI = FindFirstObjectByType<QuestUI>();
        InventoryController.Instance.OnInventoryChanged += CheckInventoryForQuests;
    }

    private void Start()
    {
        foreach (Quest quest in allQuests)
        {
            if (quest.isUnlocked)
            {
                AcceptQuest(quest);
            }
        }
    }

    public void AcceptQuest(Quest quest)
    {
        if (IsQuestActive(quest.questID)) return;
        
        activeQuests.Add(new QuestProgress(quest));
        
        CheckInventoryForQuests();
        questUI.UpdateQuestUI();
    }
    
    public bool IsQuestActive(string questID) => activeQuests.Exists(q => q.quest.questID == questID);

    public void CheckInventoryForQuests()
    {
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();

        foreach (QuestProgress quest in activeQuests)
        {
            foreach (QuestObjective questObjective in quest.objectives)
            {
                // if (questObjective.type != ObjectiveType.CollectItem) continue;
                if (!int.TryParse(questObjective.objectiveID, out int itemID)) continue;
                
                int newAmount = itemCounts.TryGetValue(itemID, out int count) ? Mathf.Min(count, questObjective.reqiredAmount) : 0;

                if (questObjective.currentAmount != newAmount)
                {
                    questObjective.currentAmount = newAmount;
                }
            }
        }
        
        questUI.UpdateQuestUI();
    }

    public bool IsQuestCompleted(string questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.QuestID == questID);
        return quest != null && quest.objectives.TrueForAll(o => o.IsCompleted);
    }

    public void HandingInQuest(string questID)
    {
        if (!RemoveRequiredItemsFromInventory(questID)) return;
        
        QuestProgress quest = activeQuests.Find(q => q.QuestID == questID);

        if (quest != null)
        {
            handingQuestIDs.Add(questID);
            activeQuests.Remove(quest);
            RewardController.Instance.GiveQuestReward(quest.quest);
            questUI.UpdateQuestUI();
            questUI.HideQuestDetails();
        }
    }

    public bool IsQuestHandedIn(string questID)
    {
        return handingQuestIDs.Contains(questID);
    }
    
    public bool RemoveRequiredItemsFromInventory(string questID)
    {
        QuestProgress quest = activeQuests.Find(q => q.QuestID == questID);
        
        if (quest == null) return false;
        
        Dictionary<int, int> requiredItems = new();

        foreach (QuestObjective questObjective in quest.objectives)
        {
            // if questObjective.type == ObjectiveType.CollectItem &&
            if (int.TryParse(questObjective.objectiveID, out int itemID))
            {
                requiredItems[itemID] = questObjective.reqiredAmount;
            }
        }
        
        Dictionary<int, int> itemCounts = InventoryController.Instance.GetItemCounts();

        foreach (var item in requiredItems)
        {
            if (itemCounts.GetValueOrDefault(item.Key) < item.Value)
            {
                return false;
            }
        }

        foreach (var itemRequirement in requiredItems)
        {
            InventoryController.Instance.RemoveItemsFromInventory(itemRequirement.Key, itemRequirement.Value);
        }

        return true;
    }
    
    public void LoadQuestProgress(List<QuestProgress> savedQuests)
    {
        activeQuests = savedQuests ?? new();
        
        CheckInventoryForQuests();
        questUI.UpdateQuestUI();
    }
}
