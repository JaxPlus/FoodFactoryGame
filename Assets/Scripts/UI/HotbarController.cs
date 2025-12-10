using System;
using System.Collections.Generic;
using UnityEngine;

public class HotbarController : MonoBehaviour
{
    public GameObject[] panels;
    private Dictionary<BuildingCategory, int> slotCount = new();
    private int currentPanelActive = 0;

    private BuildingDictionary buildingDictionary;

    private void Awake()
    {
        buildingDictionary = FindFirstObjectByType<BuildingDictionary>();
    }

    void Start()
    {
        slotCount.Add(BuildingCategory.Machines, 0);
        slotCount.Add(BuildingCategory.Decoration, 0);

        ActivateHotbarPanel(0);

        foreach (var building in buildingDictionary.buildingPrefabs)
        {
            slotCount[building.Category] += 1;
        }

        int currentBuilding = 0;
        int i = 0;
        foreach (var slot in slotCount)
        {
            for (int j = 0; j < slot.Value; j++)
            { 
                Instantiate(buildingDictionary.GetComponent<BuildingDictionary>().buildingPrefabs[currentBuilding].gameObject, panels[i].transform);
                currentBuilding++;
            }
            i++;
        }
    }

    public void ActivateHotbarPanel(int hotbarPanelNum)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panels[hotbarPanelNum].SetActive(true);
        currentPanelActive = hotbarPanelNum;
    }
}
