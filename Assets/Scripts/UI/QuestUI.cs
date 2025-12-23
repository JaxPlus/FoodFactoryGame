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

    public Quest testQuest;
    public int testQuestAmount;
    private List<QuestProgress> testQuests = new();
    
    void Start()
    {
        for (int i = 0; i < testQuestAmount; i++)
        {
            testQuests.Add(new QuestProgress(testQuest));
        }
        
        UpdateQuestUI();
        ShowQuestDetails(testQuests[0]);
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
        requestTitle.GetComponent<TMP_Text>().text = quest.quest.questName;
        
        foreach (Transform child in objectiveList.transform)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var objective in quest.objectives)
        {
            GameObject objTextGO = Instantiate(objectiveTextPrefab, objectiveList.transform);
            TMP_Text objText = objTextGO.GetComponent<TMP_Text>();
            objText.text = $"{objective.description} ({objective.currentAmount}/{objective.reqiredAmount})";
        }
    }
}
