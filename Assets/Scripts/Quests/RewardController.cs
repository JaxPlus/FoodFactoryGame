using System;
using UnityEngine;

public class RewardController : MonoBehaviour
{
    public static RewardController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void GiveQuestReward(Quest quest)
    {
        if (quest?.questRewards == null) return;

        foreach (var reward in quest.questRewards)
        {
            switch (reward.type)
            {
                case RewardType.Item:
                    // Item reward
                    GiveItemReward(reward.rewardID, reward.amount);
                    break;
                case RewardType.Building:
                    // Unlock next Building
                    break;
                case RewardType.NextQuest:
                    // Unlock next quest
                    break;
                case RewardType.Reputation:
                    // Type for future reputation system if I will do it
                    throw new NotImplementedException("Feature not implemented yet");
                case RewardType.Custom:
                    break;
            }
        }
    }

    public void GiveItemReward(int itemID, int amount)
    {
        var itemPrefab = FindFirstObjectByType<ItemDictionary>()?.GetItemPrefab(itemID);
        
        if (itemPrefab == null) return;

        for (int i = 0; i < amount; i++)
        {
            InventoryController.Instance.AddItem(itemPrefab);
        }
    }
}
