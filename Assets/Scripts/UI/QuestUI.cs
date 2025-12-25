using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    public Transform questListContent;
    public GameObject questEntryPrefab;
    public GameObject objectiveTextPrefab;
    public GameObject objectiveList;
    public GameObject requestTitle;
    public GameObject completeQuestBtn;
    public GameObject rewardsText;
    
    void Start()
    {
        UpdateQuestUI();
        ShowQuestDetails(QuestController.Instance.activeQuests[0]);
    }

    public void UpdateQuestUI()
    {
        foreach (Transform child in questListContent)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var quest in QuestController.Instance.activeQuests)
        {
            GameObject entry = Instantiate(questEntryPrefab, questListContent);
            TMP_Text questNameText = entry.transform.Find("QuestNameText").GetComponent<TMP_Text>();
            Button btn = entry.GetComponent<Button>();
            btn.onClick.AddListener(() => ShowQuestDetails(quest));

            questNameText.text = quest.quest.questName;
        }
    }

    public void ShowQuestDetails(QuestProgress quest)
    {
        foreach (Transform child in objectiveList.transform)
        {
            Destroy(child.gameObject);
        }
        
        requestTitle.GetComponent<TMP_Text>().text = quest.quest.questName;
        completeQuestBtn.GetComponent<Button>().onClick.AddListener(() => QuestController.Instance.HandingInQuest(quest.QuestID));
        
        foreach (var objective in quest.objectives)
        {
            GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList.transform);
            TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
            objText.text = $"{objective.description} ({objective.currentAmount}/{objective.reqiredAmount})";
        }

        rewardsText.GetComponent<TMP_Text>().text = "Rewards: ";
        foreach (var rewards in quest.quest.questRewards)
        {
            rewardsText.GetComponent<TMP_Text>().text += rewards.type;
        }
    }
}
