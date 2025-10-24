using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Building : MonoBehaviour, IPointerClickHandler
{
    public int ID;
    public string Name;
    public BuildingCategory Category;
    public BuildingData buildingData;
    private Dictionary<int, int> ingredientsDict;
    private int ingredientsCount;
    
    void Start()
    {
        ingredientsDict = new Dictionary<int, int>();
        SetItemDictionary();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        // @TODO Dokończyć to czy mamy wybrany dany budynek czy nie
        if (BuildingSystem.Instance.currentBuildingID == -1)
        {
            StartBuilding();
        }
        else if (BuildingSystem.Instance.currentBuildingID != ID)
        {
            BuildingSystem.Instance.StopPreview();
            StartBuilding();
        }
        else
        {
            BuildingSystem.Instance.StopPreview();
        }
    }

    private void StartBuilding()
    {
        var invItems = InventoryController.Instance.GetInventorySaveItems();

        for (int i = 0; i < invItems.Count; i++)
        {
            if (ingredientsDict.ContainsKey(invItems[i].itemID))
            {
                ingredientsDict[invItems[i].itemID] -= invItems[i].quantity;
            }
        }

        foreach (var i in ingredientsDict)
        {
            if (i.Value <= 0)
            {
                ingredientsCount--;
            }
        }

        if (ingredientsCount == 0)
        {
            SetItemDictionary();
            BuildingSystem.Instance.SetPreview(buildingData, ID);
            Debug.Log("Building finished.");

            foreach (GameObject ingredient in buildingData.Cost)
            {
                InventoryController.Instance.RemoveItem(ingredient);
            }
        }
        else
        {
            Debug.Log("Not enough items.");
        }
    }

    private void SetItemDictionary()
    {
        ingredientsDict.Clear();
        foreach (GameObject items in buildingData.Cost)
        {
            var currItem = items.GetComponent<Item>();

            if (!ingredientsDict.TryAdd(currItem.ID, 1))
            {
                ingredientsDict[currItem.ID]++;
            }
        }

        ingredientsCount = ingredientsDict.Count;
    }
}
