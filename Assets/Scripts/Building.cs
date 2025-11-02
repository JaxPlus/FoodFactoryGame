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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        if (BuildingSystem.Instance.currentBuildingID == -1)
        {
            BuildingSystem.Instance.SetPreview(buildingData, ID);
        }
        else if (BuildingSystem.Instance.currentBuildingID != ID)
        {
            BuildingSystem.Instance.StopPreview();
            BuildingSystem.Instance.SetPreview(buildingData, ID);
        }
        else
        {
            BuildingSystem.Instance.StopPreview();
        }
    }
}
